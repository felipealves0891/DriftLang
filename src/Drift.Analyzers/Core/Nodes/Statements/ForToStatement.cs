using System;
using Drift.Analyzers.Core.Nodes.Declarations;
using Drift.Analyzers.Core.Nodes.Expressions;
using Drift.Analyzers.Core.Location;
using Drift.Analyzers.Core.Nodes.Literals;
using Drift.Analyzers.CodeGen;

namespace Drift.Analyzers.Core.Nodes.Statements;

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
            var interpreter = context.CreateFunction(this);

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

    public override void GenerateCode(Stack<Instruction> instructions)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return $"for {Declaration} to {Until} {{\n{base.ToString()}\n}}";
    }
}
