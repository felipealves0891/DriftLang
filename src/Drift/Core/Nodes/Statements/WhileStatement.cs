using System;
using Drift.Core.Nodes.Expressions;
using Drift.Core.Location;
using System.Text;

namespace Drift.Core.Nodes.Statements;

public class WhileStatement : BlockStatement
{
    public WhileStatement(
        ExpressionNode expression,
        StatementNode[] nodes,
        SourceLocation location) : base(nodes, location)
    {
        Expression = expression;
        Expression.Parent = this;
    }

    public ExpressionNode Expression { get; }

    public override DriftNode[] Children => Nodes;

    public override void Execute(IExecutionContext context)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        var block = string.Join('\n', Nodes.Select(x => $"\t{x}"));
        return $"while {Expression} {{\n{block}\n}}";
    }
}
