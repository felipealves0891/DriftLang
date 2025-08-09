using System;
using Drift.Analyzers.Core.Nodes;
using Drift.Analyzers.Core.Nodes.Statements;
using Drift.Analyzers.Semantic.Diagnostic;

namespace Drift.Analyzers.Semantic;

public interface ISemanticAnalyzer
{
    DiagnosticAggregator Analyze(DriftNode node);
}
