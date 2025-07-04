using System;
using Drift.Core.Ast.Types;
using Drift.Core.Location;

namespace Drift.Core.Nodes.Helpers;

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
    public override string ToString()
    {
        return Value;
    }
}
