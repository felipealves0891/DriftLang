using Drift.Analyzers.Core.Nodes.Expressions;
using Drift.Analyzers.Core.Location;
using Drift.Analyzers.Core.Nodes.Literals;
using Drift.Analyzers.Core.Helpers;
using Drift.Analyzers.CodeGen;

namespace Drift.Analyzers.Core.Nodes.Statements;

public class IfStatement : BlockStatement
{
    public IfStatement(
        ExpressionNode expression,
        StatementNode[] trueBlock,
        SourceLocation location)
        : base(trueBlock, location)
    {
        Expression = expression;
    }

    public IfStatement(
        ExpressionNode expression,
        StatementNode[] trueBlock,
        StatementNode[] elseBlock,
        SourceLocation location)
        : base(trueBlock, location)
    {
        Expression = expression;
        var elseLocation = elseBlock.First().Location.Join(elseBlock.Last().Location);
        ElseBlock = new ElseStatement(elseBlock, elseLocation);
        ElseBlock.Parent = this;
    }

    public ExpressionNode Expression { get; }
    public ElseStatement? ElseBlock { get; }

    public override DriftNode[] Children
        => ElseBlock is not null ? [Expression, ..Nodes, ..ElseBlock.Nodes] : [Expression, ..Nodes];

    public override void Execute(IExecutionContext context)
    {
        var result = (BooleanLiteral)Expression.Evaluate(context);
        if (result.Value)
        {
            var interpreter = context.CreateFunction(this);
            var value = interpreter.Invoke(new Dictionary<string, IDriftValue>());
            if (value is not null)
                throw new BreakFlow(value);
        }
        else if (ElseBlock is not null)
        {
            ElseBlock.Execute(context);
        }
    }

    public override void GenerateCode(Stack<Instruction> instructions)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        var elseBlock = ElseBlock is not null
                    ? $"else{{\n{string.Join(' ', ElseBlock.Nodes.Select(x => $"\t{x}"))}}}"
                    : "";

        return $"if {Expression} {{ {base.ToString()} }}{elseBlock}";
    }
}
