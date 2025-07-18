using System;
using Drift.Core.Nodes.Expressions;
using Drift.Core.Nodes.Statements;
using Drift.Core.Location;
using Drift.Core.Ast.Types;

namespace Drift.Core.Nodes.Declarations;

public class OnDeclaration : FunctionDeclaration
{
    public OnDeclaration(
        string identifier,
        VariableDeclaration[] parameters,
        IDataType type,
        StatementNode[] nodes,
        SourceLocation location) : base(identifier, parameters, type, nodes, location)
    {
    }

    public override void Declare(IExecutionContext context)
    {}

    public override void Execute(IExecutionContext context)
    {
        context.Subscribe(Identifier, context.CreateFunction(this));
    }

    public override string ToString()
    {
        var parameters = string.Join(',', Parameters.Select(x => $"{x.Identifier}:{x.Type}"));
        return $"on {Identifier} ({parameters}) {{...}}";
    }
}
