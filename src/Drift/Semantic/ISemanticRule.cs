using System;
using Drift.Core.Nodes;
using Drift.Semantic.Diagnostic;
using Drift.Semantic.References;
using Drift.Semantic.Symbols;

namespace Drift.Semantic;

public interface ISemanticRule
{
    void Apply(DriftNode node, SymbolTable table, ReferenceMapper references, DiagnosticAggregator aggregator);
    void PrepareScope();
}
