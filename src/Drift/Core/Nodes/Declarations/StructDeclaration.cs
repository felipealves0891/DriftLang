using System;
using Drift.Core.Location;

namespace Drift.Core.Nodes.Declarations;

public class StructDeclaration : DeclarationNode
{
    public StructDeclaration(
        string identifier,
        StructFieldDeclaration[] fields,
        SourceLocation location)
        : base(identifier, location)
    {
        Fields = fields;
        foreach (var field in Fields)
            field.Parent = this;
    }

    public StructFieldDeclaration[] Fields { get; }

    public override DriftNode[] Children => Fields;

    public override void Declare(IExecutionContext context)
    {
    }

    public override void Execute(IExecutionContext context)
    {
    }

    public override string ToString()
    {
        var fields = string.Join(",\n", Fields.Select(x => $"\t{x.Identifier}: {x.Type}"));
        return $"type {Identifier} {{\n{fields}\n}}";
    }
}
