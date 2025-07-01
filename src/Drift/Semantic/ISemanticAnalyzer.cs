using System;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Statements;
using Drift.Semantic.Diagnostic;

namespace Drift.Semantic;

public interface ISemanticAnalyzer
{
    DiagnosticAggregator Analyze(DriftNode node);
}
