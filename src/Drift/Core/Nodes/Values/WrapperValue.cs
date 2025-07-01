using System;
using Drift.Core.Ast.Types;
using Drift.Core.Location;
using Drift.Core.Nodes.Expressions;

namespace Drift.Core.Nodes.Values;

public class WrapperValue : ExpressionNode, IDriftValue
{
    public ExpressionNode Expression { get; set; }

    public WrapperValue(
        ExpressionNode expression,
        SourceLocation location)
        : base(location)
    {
        Expression = expression;
        Type = DriftEnv.TypeRegistry.Resolve("any");
    }

    public object Unwrap => Expression;

    public IDataType Type { get; set; }

    public override DriftNode[] Children => [Expression];

    public override IDriftValue Evaluate(IExecutionContext context)
    {
        return Expression.Evaluate(context);
    }
}
