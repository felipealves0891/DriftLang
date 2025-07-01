using System;
using Drift.Core.Ast.Types;
using Drift.Core.Location;

namespace Drift.Core.Nodes.Literals;

public abstract class LiteralNode : DriftNode, IDriftValue
{
    protected LiteralNode(
        object unwrap,
        string typeName,
        SourceLocation location) : base(location)
    {
        Type = DriftEnv.TypeRegistry.Resolve(typeName);
        Unwrap = unwrap;
    }

    public IDataType Type { get; }
    public object Unwrap { get; }
}

public abstract class LiteralNode<T> : LiteralNode
    where T : notnull
{
    protected LiteralNode(
        object unwrap,
        string typeName,
        SourceLocation location)
        : base(unwrap, typeName, location)
    {
        Value = ValueParse(unwrap);
    }

    public T Value { get; }

    protected abstract T ValueParse(object value);

    public override string ToString()
    {
        return Value.ToString() ?? "Cannot parsed value";
    }
}
