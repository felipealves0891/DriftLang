using System;
using Drift.Core.Ast.Types;
using Drift.Core.Location;

namespace Drift.Core.Nodes.Declarations;

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

    public override string ToString()
    {
        return $"{Identifier}: {Type.Name}";
    }
}
