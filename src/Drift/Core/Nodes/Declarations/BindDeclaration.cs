using System;
using Drift.Core.Nodes.Expressions;
using Drift.Core.Ast.Types;
using Drift.Core.Location;

namespace Drift.Core.Nodes.Declarations;

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

    public override string ToString()
    {
        return $"bind {Identifier}: {Type} -> {Expression};";
    }
}
