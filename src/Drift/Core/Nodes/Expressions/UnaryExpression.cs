using System;
using Drift.Core.Location;
using Drift.Core.Nodes.Literals;

namespace Drift.Core.Nodes.Expressions;

public class UnaryExpression : ExpressionNode
{
    public UnaryExpression(
        ExpressionNode value,
        string indicator,
        SourceLocation location)
        : base(location)
    {
        Value = value;
        Value.Parent = this;
        Indicator = indicator;
    }

    public ExpressionNode Value { get; }
    public string Indicator { get; }
    public override DriftNode[] Children => [Value];

    public override IDriftValue Evaluate(IExecutionContext context)
    {
        if (Indicator == "not")
        {
            var result = (BooleanLiteral)Value.Evaluate(context);
            return new BooleanLiteral(!result.Value, Location);
        }
        else
        {
            var result = Value.Evaluate(context);
            if (result is IntegerLiteral integer)
            {
                var negative = integer.Value - integer.Value * 2;
                return new IntegerLiteral(negative, Location);
            }
            else
            {
                var resultFloat = (FloatLiteral)result;
                var negative = resultFloat.Value - resultFloat.Value * 2;
                return new FloatLiteral(negative, Location);
            }
        }
    }

    public override string ToString()
    {
        if (Indicator == "not")
            return $"{Indicator} {Value}";
        else
            return $"{Indicator}{Value}";
    }
}
