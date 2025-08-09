using System;
using Drift.Analyzers.CodeGen;
using Drift.Analyzers.Core.Ast.Types;
using Drift.Analyzers.Core.Location;

namespace Drift.Analyzers.Core.Nodes.Helpers;

public class IdentifierNode : DriftNode, IDriftValue, IIdentifier
{
    public IdentifierNode(
        string value,
        SourceLocation location) : base(location)
    {
        Value = value;
    }

    public string Identifier => Value;
    public string Value { get; }
    public IDataType Type { get; set; } = null!;
    public object Unwrap => null!;
    public override DriftNode[] Children => [];

    public override void GenerateCode(Stack<Instruction> instructions)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return Value;
    }
}
