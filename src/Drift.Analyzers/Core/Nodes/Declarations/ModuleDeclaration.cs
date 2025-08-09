using System;
using Drift.Analyzers.CodeGen;
using Drift.Analyzers.Core.Location;
using Drift.Analyzers.Core.Nodes.Statements;

namespace Drift.Analyzers.Core.Nodes.Declarations;

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
        module.Initialize();
        
        context.Set(Identifier, module);
    }

    public override void GenerateCode(Stack<Instruction> instructions)
    {
        throw new NotImplementedException();
    }
}
