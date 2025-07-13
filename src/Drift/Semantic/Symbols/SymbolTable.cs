using System;
using System.Collections.Concurrent;
using Drift.Core.Helpers;

namespace Drift.Semantic.Symbols;

public class SymbolTable
{
    private readonly ConcurrentStack<string> _modules = new();
    private readonly ConcurrentStack<ConcurrentDictionary<string, Symbol>> _scopes = new();

    public SymbolTable()
    {
        EnterScope();
        EnterModule("main");
    }

    private ConcurrentDictionary<string, Symbol> CurrentScope
        => _scopes.TryPeek(out var table) ? table : new ConcurrentDictionary<string, Symbol>();

    public IDisposable EnterModule(string name)
    {
        _modules.Push(name);
        return new ExiterScope(ExitModule);
    }

    public void ExitModule()
    {
        _modules.TryPop(out var _);
    }

    public IDisposable EnterScope()
    {
        _scopes.Push(new());
        return new ExiterScope(ExitScope);
    }

    public void ExitScope()
    {
        _scopes.TryPop(out var _);
    }

    public bool Declare(Symbol symbol)
    {
        var symbolName = ResolveName(symbol.Identifier);
        var current = CurrentScope;
        if (current.ContainsKey(symbolName)) return false;
        current[symbolName] = symbol;
        return true;
    }

    public Symbol? Resolve(string name)
    {
        var symbolName = ResolveName(name);
        foreach (var scope in _scopes)
            if (scope.TryGetValue(symbolName, out var sym))
                return sym;

        return null;
    }

    public IEnumerable<SymbolDependence> ResolveParameters(string name)
    {
        var symbol = Resolve(name);
        if (!symbol.HasValue)
            return Enumerable.Empty<SymbolDependence>();

        return symbol.Value.Dependences;
    }

    private string ResolveName(string name)
    {
        var module = _modules.TryPeek(out var result) ? result : "main";
        return $"{module}@{name}";
    }
}
