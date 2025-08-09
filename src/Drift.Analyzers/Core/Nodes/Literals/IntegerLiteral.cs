using System;
using Drift.Analyzers.CodeGen;
using Drift.Analyzers.Core.Ast.Types;
using Drift.Analyzers.Core.Location;

namespace Drift.Analyzers.Core.Nodes.Literals;

public class IntegerLiteral : LiteralNode<int>
{
    public IntegerLiteral(
        object unwrap,
        SourceLocation location) : base(unwrap, "int", location)
    {
    }

    public override DriftNode[] Children => [];

    public override void GenerateCode(Stack<Instruction> instructions)
    {
        throw new NotImplementedException();
    }

    protected override int ValueParse(object value)
    {
        return (int)Convert.ChangeType(value, typeof(int));
    }
}
