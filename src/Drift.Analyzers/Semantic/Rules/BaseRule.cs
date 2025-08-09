using System;
using Drift.Analyzers.Core;
using Drift.Analyzers.Core.Nodes;
using Drift.Analyzers.Semantic.Diagnostic;
using Drift.Analyzers.Semantic.References;
using Drift.Analyzers.Semantic.Symbols;

namespace Drift.Analyzers.Semantic.Rules;

public abstract class BaseRule : ISemanticRule
{
    private readonly Dictionary<Type, Action<DriftNode>> _handlers;

    protected BaseRule()
    {
        _handlers = new();
    }
    
    protected DiagnosticAggregator Aggregator { get; private set; } = null!;
    protected SymbolTable Table { get; private set; } = null!;
    protected ReferenceMapper References { get; private set; } = null!;

    protected void AddHandler<TNode>(Action<DriftNode> action) =>
        _handlers[typeof(TNode)] = action;

    public void Apply(DriftNode node, SymbolTable table, ReferenceMapper references, DiagnosticAggregator aggregator)
    {
        Table = table;
        Aggregator = aggregator;
        References = references;

        var type = node.GetType();
        PrepareApply(Table);
        if (_handlers.TryGetValue(type, out var handler))
            handler(node);
        AfterApply(node);

    }

    public virtual void PrepareScope()
    { }

    public virtual void PrepareApply(SymbolTable table)
    { }
    
    public virtual void AfterApply(DriftNode node)
    {}

}
