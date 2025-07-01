using System;
using Drift.Core.Ast.Types;
using Drift.Core.Location;

namespace Drift.Core.Nodes.Literals;

public class BooleanLiteral : LiteralNode<bool>
{

    public BooleanLiteral(
        bool unwrap,
        SourceLocation location) : base(unwrap, "bool", location)
    {
    }

    public override DriftNode[] Children => [];

    protected override bool ValueParse(object value)
    {
        return (bool)value;
    }
}
