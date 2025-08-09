using System;
using Drift.Analyzers.CodeGen;
using Drift.Analyzers.Core.Ast.Types;
using Drift.Analyzers.Core.Location;

namespace Drift.Analyzers.Core.Nodes.Declarations;

public class StructFieldDeclaration : DeclarationNode
{
    public StructFieldDeclaration(
        string identifier,
        IDataType type,
        SourceLocation location)
        : base(identifier, location)
    {
        Type = type;
    }

    public IDataType Type { get; }

    public override DriftNode[] Children => [];

    public override void Declare(IExecutionContext context)
    {
    }

    public override void Execute(IExecutionContext context)
    {
    }

    public override void GenerateCode(Stack<Instruction> instructions)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return $"{Identifier}: {Type.Name}";
    }
}
