using System;
using Drift.Analyzers.Core.Nodes.Expressions;
using Drift.Analyzers.Core.Location;

namespace Drift.Analyzers.Core.Nodes.Statements;

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
