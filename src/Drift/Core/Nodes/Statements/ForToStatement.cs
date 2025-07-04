using System;
using Drift.Core.Nodes.Declarations;
using Drift.Core.Nodes.Expressions;
using Drift.Core.Location;
using Drift.Core.Nodes.Literals;

namespace Drift.Core.Nodes.Statements;

public class ForToStatement : BlockStatement
{
    public ForToStatement(
        VariableDeclaration declaration,
        ExpressionNode until,
        StatementNode[] nodes,
        SourceLocation location)
        : base(nodes, location)
    {
        Declaration = declaration;
        Declaration.Parent = this;
        Until = until;
        Until.Parent = this;
    }

    public VariableDeclaration Declaration { get; }
    public ExpressionNode Until { get; }
    public override DriftNode[] Children => Nodes;

    public override void Execute(IExecutionContext context)
    {
        using (context.EnterScope())
        {
            var interpreter = context.CreateInterpreter(this);

            Declaration.Declare(context);
            Declaration.Execute(context);

            var value = (IntegerLiteral)context.Get(Declaration.Identifier);
            var until = (IntegerLiteral)Until.Evaluate(context);

            while (value.Value < until.Value)
            {
                interpreter.Invoke(new Dictionary<string, IDriftValue>());
                value = new IntegerLiteral(value.Value + 1, value.Location);
                context.Set(Declaration.Identifier, value);
            }
        }
    }
    

    public override string ToString()
    {
        return $"for {Declaration} to {Until} {{\n{base.ToString()}\n}}";
    }
}
