using System;
using Drift.Core.Location;
using Drift.Core.Nodes.Values;
using Drift.Core.Types;

namespace Drift.Core.Nodes.Expressions;

public class StructAccessExpression : ExpressionNode, IIdentifier
{
    public StructAccessExpression(
        string instance,
        string property,
        SourceLocation location) : base(location)
    {
        Instance = instance;
        Property = property;
    }

    public string Identifier => Instance;
    public string Instance { get; }
    public string Property { get; }

    public override DriftNode[] Children => [];

    public override IDriftValue Evaluate(IExecutionContext context)
    {
        var instance = (StructInstanceValue)context.Get(Instance);
        return instance.Properties[Property].Evaluate(context);
    }

    public override string ToString()
    {
        return $"{Instance}->{Property}";
    }
}
