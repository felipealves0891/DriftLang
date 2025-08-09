using System;
using Drift.Analyzers.Core.Location;
using Drift.Analyzers.Core.Nodes.Helpers;
using Drift.Analyzers.Core.Nodes.Literals;
using Drift.Analyzers.Core.Nodes.Values;

namespace Drift.Analyzers.Core.Nodes.Expressions;

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
