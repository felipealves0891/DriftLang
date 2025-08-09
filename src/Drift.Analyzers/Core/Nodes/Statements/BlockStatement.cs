using System;
using System.Text;
using Drift.Analyzers.Core.Location;

namespace Drift.Analyzers.Core.Nodes.Statements;

public abstract class BlockStatement : StatementNode
{
    protected BlockStatement(
        StatementNode[] nodes,
        SourceLocation location) : base(location)
    {
        Nodes = nodes;
        foreach (var node in Nodes)
            node.Parent = this;
    }

    public StatementNode[] Nodes { get; }

    public override string ToString()
    {
        return string.Join('\n', Nodes.Select(x => $"\t{x}"));
    }

}
