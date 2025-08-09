using System;
using Drift.Analyzers.Core.Location;

namespace Drift.Analyzers.Core.Nodes.Expressions;

public class FunctionCallExpression : ExpressionNode, IIdentifier
{
    public FunctionCallExpression(
        string identifier,
        ExpressionNode[] arguments,
        SourceLocation location)
        : base(location)
    {
        Identifier = identifier;
        Arguments = new();

        for (int i = 0; i < arguments.Length; i++)
        {
            arguments[i].Parent = this;
            Arguments[i.ToString()] = arguments[i];
        }   
    }

    public string Identifier { get; }
    public Dictionary<string, ExpressionNode> Arguments { get; }
    public override DriftNode[] Children => Arguments.Values.ToArray();

    public override IDriftValue Evaluate(IExecutionContext context)
    {
        var fun = (IDriftFunction)context.Get(Identifier);
        var args = new Dictionary<string, IDriftValue>();
        foreach (var keyValue in Arguments)
            args[keyValue.Key] = keyValue.Value.Evaluate(context);    

        return fun.Invoke(args) ?? null!;
    }

    public override string ToString()
    {
        var parameters = Arguments.Select(x => x.ToString());
        return $"{Identifier}({string.Join(',', parameters)})";
    }
}
