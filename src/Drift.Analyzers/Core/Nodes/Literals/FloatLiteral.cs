using System;
using System.Globalization;
using Drift.Analyzers.Core.Ast.Types;
using Drift.Analyzers.Core.Location;

namespace Drift.Analyzers.Core.Nodes.Literals;

public class FloatLiteral : LiteralNode<decimal>
{
    public FloatLiteral(
        object value,
        SourceLocation location) 
        : base(value, "float", location)
    {
        
    }

    public override DriftNode[] Children => [];

    protected override decimal ValueParse(object value)
    {
        return (decimal)Convert.ChangeType(value, TypeCode.Decimal);
    }
}
