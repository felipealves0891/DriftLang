using System;
using Drift.Analyzers.Core.Ast.Types;
using Drift.Analyzers.Core.Location;

namespace Drift.Analyzers.Core.Nodes.Literals;

public class StringLiteral : LiteralNode<string>
{
    public StringLiteral(
        object unwrap,
        SourceLocation location) : base(unwrap, "string", location)
    {
    }

    protected override string ValueParse(object value)
    {
        return value.ToString() ?? "";
    }

    public override DriftNode[] Children => [];

    public override string ToString()
    {
        return $"'{Value}'";
    }

}
