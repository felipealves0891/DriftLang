using System;
using Drift.Analyzers.CodeGen;
using Drift.Analyzers.Core.Ast.Types;
using Drift.Analyzers.Core.Location;

namespace Drift.Analyzers.Core.Nodes.Declarations;

public class EventDeclaration : DeclarationNode
{
    public EventDeclaration(
        string identifier,
        VariableDeclaration[] parameters,
        SourceLocation location)
        : base(identifier, location)
    {
        Parameters = parameters;
        foreach (var parameter in Parameters)
            parameter.Parent = this;
    }

    public VariableDeclaration[] Parameters { get; }

    public override DriftNode[] Children => [];

    public override void Declare(IExecutionContext context)
    {
        context.Event(Identifier);
    }

    public override void Execute(IExecutionContext context)
    {}

    public override void GenerateCode(Stack<Instruction> instructions)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        var parameters = Parameters.Select(x => $"{x.Identifier}: {x.Type.Name}");
        return $"event {Identifier}({string.Join(',',parameters)});";
    }
}
