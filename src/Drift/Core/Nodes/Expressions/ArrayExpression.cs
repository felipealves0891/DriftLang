using System;
using Drift.Core.Location;
using Drift.Core.Nodes.Values;

namespace Drift.Core.Nodes.Expressions;

public class ArrayExpression : ExpressionNode
{
    public ArrayExpression(
        ExpressionNode[] expressions,
        SourceLocation location) : base(location)
    {
        Expressions = expressions;
    }

    public override DriftNode[] Children => Expressions;

    public ExpressionNode[] Expressions { get; }

    public override IDriftValue Evaluate(IExecutionContext context)
    {
        var values = Expressions.Select(x => x.Evaluate(context)).ToArray();
        return new ArrayValue(values, Location);
    }
}
