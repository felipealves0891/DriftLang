using System;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Declarations;
using Drift.Core.Nodes.Expressions;
using Drift.Core.Nodes.Helpers;
using Drift.Core.Nodes.Statements;
using Drift.Semantic.References;

namespace Drift.Semantic.Rules;

public class ConsistencyRule : BaseRule
{
    public ConsistencyRule()
    {
        AddHandler<AssignmentStatement>(AssignmentStatementApply);
    }

    private void AssignmentStatementApply(DriftNode node)
    {
        var assignment = (AssignmentStatement)node;
        var symbol = Table.Resolve(assignment.Identifier);
        if (symbol.HasValue && symbol.Value.Immutable)
            Aggregator.AddError($"Cannot change the value of a constant: {assignment.Identifier}", node.Location);
    }

    public override void AfterApply(DriftNode node)
    {
        if (node is IIdentifier identifier)
        { 
            References.SetTo(
                new Reference(
                    identifier.Identifier,
                    node.GetType(),
                    identifier.Location));
        }
    }
    
}
