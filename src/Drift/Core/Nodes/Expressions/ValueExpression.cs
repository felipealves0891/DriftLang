using System;
using Drift.Core.Location;
using Drift.Core.Nodes.Helpers;

namespace Drift.Core.Nodes.Expressions;

public class ValueExpression : ExpressionNode
{
    public ValueExpression(
        IDriftValue value,
        SourceLocation location) : base(location)
    {
        Value = value;
    }

    public IDriftValue Value { get; }
    public override DriftNode[] Children => [];

    public override IDriftValue Evaluate(IExecutionContext context)
    {
        if (Value is IdentifierNode identifier)
            return (IDriftValue)context.Get(identifier.Value);
            
        return Value;
    }

    public override string ToString()
    {
        return Value.ToString() ?? "";
    }
}
