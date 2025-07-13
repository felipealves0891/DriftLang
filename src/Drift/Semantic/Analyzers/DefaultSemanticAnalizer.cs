using System;
using Drift.Core;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Declarations;
using Drift.Core.Nodes.Statements;
using Drift.Semantic.Diagnostic;
using Drift.Semantic.References;
using Drift.Semantic.Symbols;

namespace Drift.Semantic.Analyzers;

public class DefaultSemanticAnalizer : ISemanticAnalyzer
{
    private readonly IEnumerable<ISemanticRule> _rules;
    private readonly DiagnosticAggregator _diagnostic;
    private readonly SymbolTable _table;
    private readonly ReferenceMapper _references;

    public DefaultSemanticAnalizer(IEnumerable<ISemanticRule> rules)
    {
        _rules = rules;
        _diagnostic = new DiagnosticAggregator();
        _table = new SymbolTable();
        _references = new ReferenceMapper(_diagnostic);
        InitializeEnv();
    }

    private void InitializeEnv()
    {
        var functions = DriftEnv.FunctionRegistry.RegisteredFunctions;
        foreach (var name in functions)
        {
            var function = DriftEnv.FunctionRegistry.Resolve(name);
            var symbol = new Symbol(function.Name, function.Type, new());

            foreach (var parameter in function.Parameters)
            {
                var dependence = new Symbol(parameter.Key, parameter.Value, new());
                if (!_table.Declare(dependence))
                    _diagnostic.AddErrorAlreadyDeclaration(dependence.Identifier, dependence.Location);

                symbol.Dependences.Add(new SymbolDependence(dependence));
            }

            if (!_table.Declare(symbol))
                _diagnostic.AddErrorAlreadyDeclaration(symbol.Identifier, symbol.Location);

        }
    }

    public DiagnosticAggregator Analyze(DriftNode node)
    {
        foreach (var rule in _rules)
        {
            rule.Apply(node, _table, _references, _diagnostic);
        }

        using (EnterModule(node))
        using (EnterFrom(node))
        using (EnterScope(node))
        {
            foreach (var rule in _rules)
                rule.PrepareScope();

            foreach (var child in node.Children)
                _diagnostic.MergeWith(Analyze(child));
        }

        return _diagnostic;
    }

    private IDisposable? EnterScope(DriftNode node)
    {
        if (node is StatementNode)
            return _table.EnterScope();
        else
            return null;
    }

    private IDisposable? EnterFrom(DriftNode node)
    {
        if (node is BindDeclaration bind)
            return _references.EnterFrom(new Reference(bind));
        else if (node is OnDeclaration on)
            return _references.EnterFrom(new Reference(on));
        else
            return null;
    }

    private IDisposable? EnterModule(DriftNode node)
    {
        if (node is ModuleDeclaration module)
            return _table.EnterModule(module.Identifier);
        else
            return null;
    }
}
