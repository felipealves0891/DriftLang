using System;
using Drift.Core.Location;

namespace Drift.Core.Nodes.Expressions;

public class ModuleAccessExpression : ExpressionNode
{
    public ModuleAccessExpression(
        string module,
        ExpressionNode expression,
        SourceLocation location) 
        : base(location)
    {
        Module = module;
        Expression = expression;
    }

    public override DriftNode[] Children => [];
    public string Module { get; }
    public ExpressionNode Expression { get; }

    public override IDriftValue Evaluate(IExecutionContext context)
    {
        var module = (IDriftModule)context.Get(Module);
        var functionCall = (FunctionCallExpression)Expression;
        var args = new Dictionary<string, IDriftValue>();
        foreach (var keyValue in functionCall.Arguments)
            args[keyValue.Key] = keyValue.Value.Evaluate(context);

        return module.Invoke(functionCall.Identifier, args) ?? null!;

    }
}
