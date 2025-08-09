using System;
using Drift.Analyzers.CodeGen;
using Drift.Analyzers.Core.Location;
using Drift.Analyzers.Core.Nodes.Declarations;

namespace Drift.Analyzers.Core.Nodes.Statements;

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

    public override void GenerateCode(Stack<Instruction> instructions)
    {
        throw new NotImplementedException();
    }
}
