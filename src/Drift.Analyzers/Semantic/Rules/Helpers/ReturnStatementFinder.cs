using System.Collections.Immutable;
using Drift.Analyzers.Core.Ast.Types;
using Drift.Analyzers.Core.Nodes;
using Drift.Analyzers.Core.Nodes.Declarations;
using Drift.Analyzers.Core.Nodes.Statements;

namespace Drift.Analyzers.Semantic.Rules.Helpers;

public class ReturnStatementFinder
{
    private readonly List<ReturnStatement> _returns;

    private ReturnStatementFinder()
    {
        _returns = new List<ReturnStatement>();
    }

    private void InternalFind(DriftNode node)
    {
        if (node is ReturnStatement @return)
        {
            _returns.Add(@return);
            return;
        }

        if (node is DeclarationNode)
            return;
            
        foreach (var child in node.Children)
            InternalFind(child);
    }

    public static ReturnStatement[] Find(StatementNode block)
    {
        var finder = new ReturnStatementFinder();
        finder.InternalFind(block);
        return finder._returns.ToArray();
    } 
}
