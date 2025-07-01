using System.Collections;
using Drift.Core.Ast.Types;
using Drift.Core.Location;
using Drift.Core.Nodes.Expressions;

namespace Drift.Core.Nodes.Values;

public class StructInstanceValue : ValueNode
{
    public StructInstanceValue(
        IDataType type,
        Dictionary<string, ExpressionNode> properties,
        SourceLocation location)
        : base(location)
    {
        Type = type;
        Properties = properties;
    }

    public override object Unwrap => this;
    public override IDataType Type { get; }
    public Dictionary<string, ExpressionNode> Properties { get; }
    public override DriftNode[] Children => Properties.Values.ToArray();
    
    public override IEnumerator GetEnumerator()
        => Properties.Values.GetEnumerator();

    public override string ToString()
    {
        var fields = Properties.Select(x => $"\t{x.Key} = {x.Value}");
        return $"{{\n{string.Join(",\n", fields)}\n}}";
    }
}
