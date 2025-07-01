using System;
using Drift.Core.Nodes.Expressions;
using Drift.Core.Location;
using Drift.Core.Nodes.Declarations;
using Drift.Core.Ast.Types;

namespace Drift.Core.Nodes.Statements;

public class ActionStatement : BlockStatement
{
    public ActionStatement(
        VariableDeclaration[] parameters,
        IDataType type,
        StatementNode[] nodes,
        SourceLocation location) : base(nodes, location)
    {
        Parameters = parameters;
        Type = type;

        foreach (var arg in parameters)
            arg.Parent = this;
    }

    public VariableDeclaration[] Parameters { get; }
    public IDataType Type { get; }
    public override DriftNode[] Children => Nodes;
    
    public override void Execute(IExecutionContext context)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        var parameters = Parameters.Select(x => $"{x.Identifier}: {x.Type}");
        return $"({string.Join(',', parameters)}): {Type} {{\n{base.ToString()}\n}}";
    }
}
