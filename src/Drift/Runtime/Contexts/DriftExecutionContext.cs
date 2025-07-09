using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using Drift.Core;
using Drift.Core.Helpers;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Declarations;
using Drift.Core.Nodes.Expressions;
using Drift.Core.Nodes.Statements;
using Drift.Runtime.EventManager;

namespace Drift.Runtime.Contexts;

public sealed class DriftExecutionContext : IExecutionContext
{
    private readonly ConcurrentStack<ConcurrentDictionary<string, Guid>> _references = new();
    private readonly ConcurrentStack<ConcurrentDictionary<Guid, IDrift>> _variables = new();
    private readonly ConcurrentStack<ConcurrentDictionary<Guid, ExpressionNode>> _binds = new();
    private readonly DriftEventManager _eventManager;

    public DriftExecutionContext()
    {
        _references.Push(new());
        _variables.Push(new());
        _binds.Push(new());
        _eventManager = new();
    }

    private DriftExecutionContext(
        IEnumerable<ConcurrentDictionary<string, Guid>> references,
        IEnumerable<ConcurrentDictionary<Guid, IDrift>> variables,
        IEnumerable<ConcurrentDictionary<Guid, ExpressionNode>> binds,
        DriftEventManager eventManager)
    {
        _references = new(references);
        _variables = new(variables);
        _binds = new(binds);
        _eventManager = eventManager;
    }

    public ConcurrentDictionary<string, Guid> CurrentReferences
        => _references.TryPeek(out var vars)
        ? vars
        : throw new InvalidOperationException("No references available.");

    public ConcurrentDictionary<Guid, IDrift> CurrentVariables
        => _variables.TryPeek(out var vars)
        ? vars
        : throw new InvalidOperationException("No variables available.");

    public ConcurrentDictionary<Guid, ExpressionNode> CurrentBinds
        => _binds.TryPeek(out var vars)
        ? vars
        : throw new InvalidOperationException("No variables available.");

    public void Declare(string name)
    {
        if (CurrentReferences.ContainsKey(name))
            throw new InvalidOperationException($"Variable '{name}' is already declared.");

        CurrentReferences[name] = Guid.NewGuid();
    }

    public IDisposable EnterScope()
    {
        _references.Push(new());
        _variables.Push(new());
        _binds.Push(new());
        return new ExiterScope(ExitScope);
    }

    public void ExitScope()
    {
        if (!_references.TryPop(out var _))
            throw new InvalidOperationException("No variables to pop.");

        if (!_variables.TryPop(out var _))
            throw new InvalidOperationException("No variables to pop.");

        if (!_binds.TryPop(out var _))
            throw new InvalidOperationException("No binds to pop.");
    }

    public IDrift Get(string name)
    {
        if (DriftEnv.FunctionRegistry.Exists(name))
            return DriftEnv.FunctionRegistry.Resolve(name);

        var guid = GetReference(name);

        foreach (var variable in _variables)
            if (variable.TryGetValue(guid, out var value))
                return value;

        foreach (var bind in _binds)
            if (bind.TryGetValue(guid, out var value))
                return value.Evaluate(this);

        throw new KeyNotFoundException($"Variable '{name}' is not defined.");
    }

    private Guid GetReference(string name)
    {
        foreach (var reference in _references)
            if (reference.TryGetValue(name, out var value))
                return value;

        throw new KeyNotFoundException($"Variable '{name}' is not declared.");
    }

    public void Set(string name, IDrift value)
    {
        var guid = GetReference(name);
        foreach (var variables in _variables)
        {
            if (variables.ContainsKey(guid))
            {
                variables[guid] = value;
            }
        }

        CurrentVariables[guid] = value;
    }

    public void Bind(string name, ExpressionNode node)
    {
        var guid = GetReference(name);
        foreach (var binds in _binds)
        {
            if (binds.ContainsKey(guid))
            {
                binds[guid] = node;
            }
        }

        CurrentBinds[guid] = node;
    }

    public IDriftFunction CreateInterpreter(FunctionDeclaration function)
    {
        return new FunctionInterpreter(Clone(), function);
    }

    public IDriftFunction CreateInterpreter(OnDeclaration observer)
    {
        return new FunctionInterpreter(Clone(), observer);
    }

    public IDriftFunction CreateInterpreter(BlockStatement statement)
    {
        return new FunctionInterpreter(Clone(), statement);
    }

    public void Event(string name)
    {
        _eventManager.Event(name);
    }

    public void Publish(string name, IDictionary<string, IDriftValue> arguments)
    {
        _eventManager.Publish(name, arguments);
    }

    public void Subscribe(string name, IDriftFunction function)
    {
        _eventManager.Subscribe(name, function);
    }

    public IExecutionContext Clone()
    {
        return new DriftExecutionContext(
            _references.ToList(),
            _variables.ToList(),
            _binds.ToList(),
            _eventManager);
    }
}
