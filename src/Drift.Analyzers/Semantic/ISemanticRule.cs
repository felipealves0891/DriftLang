using System;
using Drift.Analyzers.Core.Nodes;
using Drift.Analyzers.Semantic.Diagnostic;
using Drift.Analyzers.Semantic.References;
using Drift.Analyzers.Semantic.Symbols;

namespace Drift.Analyzers.Semantic;

public interface ISemanticRule
{
    void Apply(DriftNode node, SymbolTable table, ReferenceMapper references, DiagnosticAggregator aggregator);
    void PrepareScope();
}
