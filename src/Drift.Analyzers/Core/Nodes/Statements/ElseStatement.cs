using System;
using Drift.Analyzers.CodeGen;
using Drift.Analyzers.Core.Helpers;
using Drift.Analyzers.Core.Location;
using Drift.Analyzers.Core.Nodes.Expressions;

namespace Drift.Analyzers.Core.Nodes.Statements;

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
        var interpreter = context.CreateFunction(this);
        var value = interpreter.Invoke(new Dictionary<string, IDriftValue>());
        if (value is not null)
            throw new BreakFlow(value);
    }

    public override void GenerateCode(Stack<Instruction> instructions)
    {
        throw new NotImplementedException();
    }
}
