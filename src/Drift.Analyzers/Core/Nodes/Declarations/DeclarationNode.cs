using System;
using Drift.Analyzers.Core.Location;
using Drift.Analyzers.Core.Nodes.Statements;

namespace Drift.Analyzers.Core.Nodes.Declarations;

public abstract class DeclarationNode : BlockStatement, IIdentifier
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
