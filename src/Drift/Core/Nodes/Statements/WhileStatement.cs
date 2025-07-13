using System;
using Drift.Core.Nodes.Expressions;
using Drift.Core.Location;
using System.Text;
using Drift.Core.Nodes.Literals;

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

    public override DriftNode[] Children => [Expression, ..Nodes];

    public override void Execute(IExecutionContext context)
    {
        using (context.EnterScope())
        {
            var interpreter = context.CreateFunction(this);
            var control = (BooleanLiteral)Expression.Evaluate(context);
            while (control.Value)
            {
                interpreter.Invoke(new Dictionary<string, IDriftValue>());
                control = (BooleanLiteral)Expression.Evaluate(context);
            }
        }
    }

    public override string ToString()
    {
        var block = string.Join('\n', Nodes.Select(x => $"\t{x}"));
        return $"while {Expression} {{\n{block}\n}}";
    }
}
