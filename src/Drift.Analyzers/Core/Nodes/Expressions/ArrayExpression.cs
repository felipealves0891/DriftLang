using System;
using Drift.Analyzers.CodeGen;
using Drift.Analyzers.Core.Location;
using Drift.Analyzers.Core.Nodes.Values;

namespace Drift.Analyzers.Core.Nodes.Expressions;

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

    public override void GenerateCode(Stack<Instruction> instructions)
    {
        throw new NotImplementedException();
    }
}
