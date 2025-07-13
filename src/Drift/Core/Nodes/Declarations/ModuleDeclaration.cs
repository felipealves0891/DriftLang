using System;
using Drift.Core.Location;
using Drift.Core.Nodes.Statements;

namespace Drift.Core.Nodes.Declarations;

public class ModuleDeclaration : DeclarationNode
{
    public ModuleDeclaration(
        string identifier,
        StatementNode[] nodes,
        SourceLocation location) 
        : base(identifier, nodes, location)
    {
    }

    public override DriftNode[] Children => Nodes;

    public override void Declare(IExecutionContext context)
    {
        context.Declare(Identifier);
    }

    public override void Execute(IExecutionContext context)
    {
        var module = context.CreateModule(this);
        context.Set(Identifier, module);
    }
}
