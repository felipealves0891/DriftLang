using System;
using Drift.Core.Location;

namespace Drift.Core.Nodes.Statements;

public abstract class StatementNode : DriftNode
{
    public StatementNode(SourceLocation location) : base(location)
    {
    }

    public abstract void Execute(IExecutionContext context);
}
