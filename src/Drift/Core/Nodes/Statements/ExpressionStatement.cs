using System;
using Drift.Core.Nodes.Expressions;
using Drift.Core.Location;

namespace Drift.Core.Nodes.Statements;

public class ExpressionStatement : StatementNode
{
    public ExpressionStatement(
        ExpressionNode expression,
        SourceLocation location)
        : base(location)
    {
        Expression = expression;
        Expression.Parent = this;
    }

    public ExpressionNode Expression { get; }

    public override DriftNode[] Children => [Expression];

    public override void Execute(IExecutionContext context)
    {
        Expression.Evaluate(context);
    }

    public override string ToString()
    {
        return $"{Expression};";
    }
}
