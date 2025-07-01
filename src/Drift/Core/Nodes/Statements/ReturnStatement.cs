using System;
using Drift.Core.Location;
using Drift.Core.Nodes.Expressions;

namespace Drift.Core.Nodes.Statements;

public class ReturnStatement : StatementNode
{
    public ReturnStatement(
        ExpressionNode expression,
        SourceLocation location) : base(location)
    {
        Expression = expression;
        Expression.Parent = this;
    }

    public IDriftValue? Output { get; private set; }    
    public ExpressionNode Expression { get; }
    public override DriftNode[] Children => [Expression];

    public override void Execute(IExecutionContext context)
    {
        Output = Expression.Evaluate(context);
    }

    public override string ToString()
    {
        return $"return {Expression};";
    }
}
