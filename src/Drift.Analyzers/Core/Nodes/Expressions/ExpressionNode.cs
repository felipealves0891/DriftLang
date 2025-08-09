using System;
using Drift.Analyzers.Core.Ast.Types;
using Drift.Analyzers.Core.Location;

namespace Drift.Analyzers.Core.Nodes.Expressions;

public abstract class ExpressionNode : DriftNode
{
    protected ExpressionNode(SourceLocation location) : base(location)
    {
    }

    public abstract IDriftValue Evaluate(IExecutionContext context);
}
