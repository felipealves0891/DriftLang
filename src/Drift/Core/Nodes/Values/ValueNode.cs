using System;
using System.Collections;
using Drift.Core.Ast.Types;
using Drift.Core.Location;

namespace Drift.Core.Nodes.Values;

public abstract class ValueNode : DriftNode, IDriftValue, IEnumerable
{
    public ValueNode(SourceLocation location) : base(location)
    {
    }

    public abstract IDataType Type { get; }
    public abstract object Unwrap { get; }
    public abstract IEnumerator GetEnumerator();
}
