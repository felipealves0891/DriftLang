using System;
using Drift.Analyzers.Core.Nodes.Expressions;
using Drift.Analyzers.Core.Ast.Types;
using Drift.Analyzers.Core.Location;
using Drift.Analyzers.CodeGen;

namespace Drift.Analyzers.Core.Nodes.Declarations;

public class VariableDeclaration : DeclarationNode
{
    public VariableDeclaration(
        string identifier,
        IDataType type,
        SourceLocation location)
        : base(identifier, location)
    {
        Type = type;
        Immutable = true;
        Async = false;
    }

    public VariableDeclaration(
        string identifier,
        IDataType type,
        bool immutable,
        bool async,
        SourceLocation location)
        : base(identifier, location)
    {
        Type = type;
        Immutable = immutable;
        Async = async;
    }

    public VariableDeclaration(
        string identifier,
        IDataType type,
        bool immutable,
        bool async,
        ExpressionNode assignment,
        SourceLocation location)
        : base(identifier, location)
    {
        Type = type;
        Immutable = immutable;
        Async = async;
        Assignment = assignment;
        Assignment.Parent = this;
    }

    public IDataType Type { get; }
    public bool Immutable { get; }
    public bool Async { get; }
    public ExpressionNode? Assignment { get; }
    public override DriftNode[] Children => Assignment is not null ? [Assignment] : [];

    public override void Declare(IExecutionContext context)
    {
        context.Declare(Identifier);
    }

    public override void Execute(IExecutionContext context)
    {
        if(Assignment is not null)
            context.Set(Identifier, Assignment.Evaluate(context));
    }

    public override void GenerateCode(Stack<Instruction> instructions)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        var typeVar = Immutable ? "const" : "let";
        var assignment = Assignment is not null ? $"= {Assignment};" : ";";
        return $"{typeVar} {Identifier}: {Type.Name} {assignment}";
    }
}
