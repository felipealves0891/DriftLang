using System;
using Drift.Analyzers.Core.Ast.Types;
using Drift.Analyzers.Core.Location;

namespace Drift.Analyzers.Core.Nodes.Literals;

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
