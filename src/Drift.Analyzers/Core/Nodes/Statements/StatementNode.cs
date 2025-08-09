using System;
using Drift.Analyzers.Core.Location;

namespace Drift.Analyzers.Core.Nodes.Statements;

public abstract class StatementNode : DriftNode
{
    public StatementNode(SourceLocation location) : base(location)
    {
    }

    public abstract void Execute(IExecutionContext context);
}
