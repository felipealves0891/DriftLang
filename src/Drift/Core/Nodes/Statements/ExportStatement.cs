using System;
using Drift.Core.Location;
using Drift.Core.Nodes.Declarations;

namespace Drift.Core.Nodes.Statements;

public class ExportStatement : StatementNode
{
    public ExportStatement(
        FunctionDeclaration declaration,
        SourceLocation location) 
        : base(location)
    {
        Declaration = declaration;
    }

    public override DriftNode[] Children => [];
    public FunctionDeclaration Declaration { get; }

    public override void Execute(IExecutionContext context)
    {
        Declaration.Declare(context);
        Declaration.Execute(context);
        context.Expose(Declaration.Identifier);
    }
}
