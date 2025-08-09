using System;
using Drift.Analyzers.Core.Nodes.Expressions;
using Drift.Analyzers.Core.Ast.Types;
using Drift.Analyzers.Core.Location;
using Drift.Analyzers.CodeGen;

namespace Drift.Analyzers.Core.Nodes.Declarations;

public class BindDeclaration : DeclarationNode
{
    public BindDeclaration(
        string identifier,
        IDataType type,
        ExpressionNode expression,
        SourceLocation location)
        : base(identifier, location)
    {
        Type = type;
        Expression = expression;
        Expression.Parent = this;
    }

    public IDataType Type { get; }
    public ExpressionNode Expression { get; }
    public override DriftNode[] Children => [Expression];

    public override void Declare(IExecutionContext context)
    {
        context.Declare(Identifier);
    }

    public override void Execute(IExecutionContext context)
    {
        context.Bind(Identifier, Expression);
    }

    public override void GenerateCode(Stack<Instruction> instructions)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return $"bind {Identifier}: {Type} -> {Expression};";
    }
}
