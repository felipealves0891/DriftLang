using System;
using Drift.Core.Helpers;
using Drift.Core.Location;
using Drift.Core.Nodes.Expressions;

namespace Drift.Core.Nodes.Statements;

public class ElseStatement : BlockStatement
{
    public ElseStatement(
        StatementNode[] nodes,
        SourceLocation location) 
        : base(nodes, location)
    {
    }

    public override DriftNode[] Children => Nodes;

    public override void Execute(IExecutionContext context)
    {
        var interpreter = context.CreateInterpreter(this);
        var value = interpreter.Invoke(new Dictionary<string, IDriftValue>());
        if (value is not null)
            throw new BreakFlow(value);
    }
}
