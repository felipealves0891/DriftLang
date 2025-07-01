using System.Collections;
using Drift.Core.Ast.Types;
using Drift.Core.Location;

namespace Drift.Core.Nodes.Values;

public class ArrayValue : ValueNode
{
    public ArrayValue(
        IDriftValue[] source,
        SourceLocation location) : base(location)
    {
        var identifier = source.Any()
            ? $"{source.First().Type.Name}[]"
            : "void";

        Type = DriftEnv.TypeRegistry.Resolve(identifier);
        Length = source.Length;
        Source = source;
    }

    public override IDataType Type { get; }
    public int Length { get; }
    public IDriftValue[] Source { get; }
    public override object Unwrap => Source;
    public override DriftNode[] Children => Source.Cast<DriftNode>().ToArray();

    public override IEnumerator GetEnumerator()
    {
        return Source.GetEnumerator();
    }

    public override string ToString()
    {
        return $"[{string.Join(',', Source.Select(x => x.ToString()))}]";
    }
}
