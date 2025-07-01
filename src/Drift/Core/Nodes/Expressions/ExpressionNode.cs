using System;
using Drift.Core.Ast.Types;
using Drift.Core.Location;

namespace Drift.Core.Nodes.Expressions;

public abstract class ExpressionNode : DriftNode
{
    protected ExpressionNode(SourceLocation location) : base(location)
    {
    }

    public abstract IDriftValue Evaluate(IExecutionContext context);
}
