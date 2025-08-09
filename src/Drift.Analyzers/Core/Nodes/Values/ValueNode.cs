using System;
using System.Collections;
using Drift.Analyzers.Core.Ast.Types;
using Drift.Analyzers.Core.Location;

namespace Drift.Analyzers.Core.Nodes.Values;

public abstract class ValueNode : DriftNode, IDriftValue, IEnumerable
{
    public ValueNode(SourceLocation location) : base(location)
    {
    }

    public abstract IDataType Type { get; }
    public abstract object Unwrap { get; }
    public abstract IEnumerator GetEnumerator();
}
