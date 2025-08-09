using System;
using Drift.Analyzers.Core.Location;
using Drift.Analyzers.Core.Nodes.Expressions;

namespace Drift.Analyzers.Core.Nodes.Statements;

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
