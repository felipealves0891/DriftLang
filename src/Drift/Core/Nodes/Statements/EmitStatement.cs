using System;
using Drift.Core.Nodes.Expressions;
using Drift.Core.Location;
using System.Collections.Concurrent;

namespace Drift.Core.Nodes.Statements;

public class EmitStatement : StatementNode, IIdentifier
{
    public EmitStatement(
        string identifier,
        Dictionary<string, ExpressionNode> arguments,
        SourceLocation location)
        : base(location)
    {
        Identifier = identifier;
        Arguments = arguments;
        foreach (var arg in arguments)
            arg.Value.Parent = this;
    }

    public string Identifier { get; }
    public Dictionary<string, ExpressionNode> Arguments { get; }
    public override DriftNode[] Children => Arguments.Values.ToArray();
    public override void Execute(IExecutionContext context)
    {
        var args = new Dictionary<string, IDriftValue>();
        foreach (var argument in Arguments)
            args[argument.Key] = argument.Value.Evaluate(context);

        context.Publish(Identifier, args);
    }

    public override string ToString()
    {
        var args = Arguments.Select(x => x.ToString());
        return $"emit {Identifier}({string.Join(',', args)});";
    }
}
