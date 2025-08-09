using System;
using Drift.Analyzers.Core.Location;

namespace Drift.Analyzers.Core.Nodes.Statements;

public class ScriptNode : BlockStatement
{
    public ScriptNode(
        StatementNode[] nodes,
        SourceLocation location) : base(nodes, location)
    {
    }

    public override DriftNode[] Children => Nodes;

    public override void Execute(IExecutionContext context)
    {
        throw new NotImplementedException();
    }
}
