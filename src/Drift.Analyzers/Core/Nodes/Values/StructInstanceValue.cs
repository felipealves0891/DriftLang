using System.Collections;
using Drift.Analyzers.CodeGen;
using Drift.Analyzers.Core.Ast.Types;
using Drift.Analyzers.Core.Location;
using Drift.Analyzers.Core.Nodes.Expressions;

namespace Drift.Analyzers.Core.Nodes.Values;

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

    public override void GenerateCode(Stack<Instruction> instructions)
    {
        throw new NotImplementedException();
    }

    public override IEnumerator GetEnumerator()
        => Properties.Values.GetEnumerator();

    public override string ToString()
    {
        var fields = Properties.Select(x => $"\t{x.Key} = {x.Value}");
        return $"{{\n{string.Join(",\n", fields)}\n}}";
    }
}
