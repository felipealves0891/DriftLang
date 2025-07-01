using System;
using Drift.Core.Location;
using Drift.Core.Nodes.Statements;

namespace Drift.Core.Nodes.Declarations;

public abstract class DeclarationNode : BlockStatement
{
    protected DeclarationNode(
        string identifier,
        StatementNode[] nodes,
        SourceLocation location)
        : base(nodes, location)
    {
        Identifier = identifier;
    }

    protected DeclarationNode(
        string identifier,
        SourceLocation location)
        : base([], location)
    {
        Identifier = identifier;
    }

    public string Identifier { get; }
    public abstract void Declare(IExecutionContext context);
}
