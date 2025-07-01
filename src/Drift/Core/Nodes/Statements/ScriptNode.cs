using System;
using Drift.Core.Location;

namespace Drift.Core.Nodes.Statements;

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
