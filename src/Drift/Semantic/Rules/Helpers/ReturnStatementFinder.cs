using System.Collections.Immutable;
using Drift.Core.Ast.Types;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Declarations;
using Drift.Core.Nodes.Statements;

namespace Drift.Semantic.Rules.Helpers;

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
