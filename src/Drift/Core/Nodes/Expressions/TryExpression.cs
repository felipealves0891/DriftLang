using System;
using Drift.Core.Nodes.Statements;
using Drift.Core.Location;

namespace Drift.Core.Nodes.Expressions;

public class TryExpression : ExpressionNode
{
    public TryExpression(
        ExpressionNode expression,
        ActionStatement success,
        ActionStatement error,
        SourceLocation location) : base(location)
    {
        Expression = expression;
        Expression.Parent = this;

        Success = success;
        Success.Parent = this;

        Error = error;
        Error.Parent = this;
    }

    public ExpressionNode Expression { get; }
    public ActionStatement Success { get; }
    public ActionStatement Error { get; }

    public override DriftNode[] Children => [Expression, Success, Error];

    public override IDriftValue Evaluate(IExecutionContext context)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return $"try {Expression} {{\n\tsuccess{Success},\n\terror{Error}\n}}";
    }
}
