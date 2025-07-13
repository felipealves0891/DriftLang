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

    public override DriftNode[] Children => [Expression];
    public string Module { get; }
    public ExpressionNode Expression { get; }

    public override IDriftValue Evaluate(IExecutionContext context)
    {
        throw new NotImplementedException();
    }
}
