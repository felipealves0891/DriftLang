using System;
using Drift.Core;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Declarations;
using Drift.Core.Nodes.Statements;
using Drift.Runtime.Contexts;

namespace Drift.Runtime;

public class DriftInterpreter
{
    private readonly IExecutionContext _context;
    private readonly BlockStatement _block;
    private readonly int _lastNodeIndex;
    private int _currentNodeIndex = 0;

    public DriftInterpreter(IExecutionContext context, BlockStatement block)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(block), "Execution context cannot be null or empty.");

        if (block == null || block.Nodes == null || block.Nodes.Count() == 0)
            throw new ArgumentNullException(nameof(block), "Block statement cannot be null or empty.");

        _block = block;
        _context = context;
        _lastNodeIndex = _block.Nodes.Count();
    }

    public DriftInterpreter(BlockStatement block)
        : this(DriftExecutionContext.ExecutionContext, block)
    {
    }

    protected IExecutionContext Context => _context;

    public IDriftValue? Interpret()
    {
        using (var _ = _context.EnterScope())
            return InternalInterpret();
    }

    protected IDriftValue? InternalInterpret()
    {
        StatementNode? node;
        while ((node = NextNode()) != null)
        {
            if (node is DeclarationNode declaration)
                declaration.Declare(_context);

            node.Execute(_context);
            if (node is ReturnStatement returnStatement)
                return returnStatement.Output;
        }
        return null;
    }

    private StatementNode? NextNode()
    {
        if (_currentNodeIndex >= _lastNodeIndex)
            return null;

        return _block.Nodes[_currentNodeIndex++];
    }

    protected void Reset()
    {
        _currentNodeIndex = 0;
    }
}
