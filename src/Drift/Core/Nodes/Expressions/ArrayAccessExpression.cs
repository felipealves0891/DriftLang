using System;
using Drift.Core.Location;
using Drift.Core.Nodes.Helpers;
using Drift.Core.Nodes.Literals;
using Drift.Core.Nodes.Values;

namespace Drift.Core.Nodes.Expressions;

public class ArrayAccessExpression : ExpressionNode, IIdentifier
{
    public ArrayAccessExpression(
        string identifier,
        ExpressionNode index,
        SourceLocation location) : base(location)
    {
        Identifier = identifier;
        Index = index;
        Index.Parent = this;
    }

    public string Identifier { get; }
    public ExpressionNode Index { get; }
    public override DriftNode[] Children => [];

    public override IDriftValue Evaluate(IExecutionContext context)
    {
        var array = (ArrayValue)context.Get(Identifier);
        var index = (IntegerLiteral)Index.Evaluate(context);
        return array.Source[index.Value];
    }

    public override string ToString()
    {
        return $"{Identifier}[{Index}]";
    }
}
