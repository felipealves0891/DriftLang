using System;
using Drift.Analyzers.Core.Nodes.Expressions;
using Drift.Analyzers.Core.Location;
using Drift.Analyzers.CodeGen;

namespace Drift.Analyzers.Core.Nodes.Statements;

public class AssignmentStatement : StatementNode, IIdentifier
{
    public AssignmentStatement(
        string identifier,
        ExpressionNode expression,
        SourceLocation location) : base(location)
    {
        Identifier = identifier;
        Expression = expression;
        Expression.Parent = this;
    }

    public string Identifier { get; }
    public ExpressionNode Expression { get; }

    public override DriftNode[] Children => [Expression];

    public override void Execute(IExecutionContext context)
    {
        context.Set(Identifier, Expression.Evaluate(context));
    }

    public override void GenerateCode(Stack<Instruction> instructions)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return $"{Identifier} = {Expression};";
    }
}
