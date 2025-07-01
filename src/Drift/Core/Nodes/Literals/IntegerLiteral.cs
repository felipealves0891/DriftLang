using System;
using Drift.Core.Ast.Types;
using Drift.Core.Location;

namespace Drift.Core.Nodes.Literals;

public class IntegerLiteral : LiteralNode<int>
{
    public IntegerLiteral(
        object unwrap,
        SourceLocation location) : base(unwrap, "int", location)
    {
    }

    public override DriftNode[] Children => [];

    protected override int ValueParse(object value)
    {
        return (int)Convert.ChangeType(value, typeof(int));
    }
}
