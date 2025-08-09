using System;
using Drift.Analyzers.Core.Nodes;
using Drift.Analyzers.Core.Nodes.Declarations;
using Drift.Analyzers.Core.Nodes.Expressions;
using Drift.Analyzers.Core.Nodes.Helpers;
using Drift.Analyzers.Core.Nodes.Statements;
using Drift.Analyzers.Semantic.References;

namespace Drift.Analyzers.Semantic.Rules;

public class ConsistencyRule : BaseRule
{
    public ConsistencyRule()
    {
        AddHandler<ReturnStatement>(ReturnStatementApply);
        AddHandler<AssignmentStatement>(AssignmentStatementApply);
    }

    private void ReturnStatementApply(DriftNode node)
    {
        bool findParentFunction(DriftNode dn)
        {
            if (dn.Parent is null)
                return false;
            else if (dn.Parent is FunctionDeclaration || dn.Parent is ActionStatement)
                return true;
            else
                return findParentFunction(dn.Parent);

        }

        var statement = (ReturnStatement)node;
        var isInsideFunction = findParentFunction(statement);
        if (!isInsideFunction)
            Aggregator.AddError("the return statement must be in a function, and never in the global context", statement.Location);
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
