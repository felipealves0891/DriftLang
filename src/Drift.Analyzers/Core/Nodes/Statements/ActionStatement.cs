using System;
using Drift.Analyzers.Core.Nodes.Expressions;
using Drift.Analyzers.Core.Location;
using Drift.Analyzers.Core.Nodes.Declarations;
using Drift.Analyzers.Core.Ast.Types;
using Drift.Analyzers.CodeGen;

namespace Drift.Analyzers.Core.Nodes.Statements;

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
    }

    public override void GenerateCode(Stack<Instruction> instructions)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        var parameters = Parameters.Select(x => $"{x.Identifier}: {x.Type}");
        return $"({string.Join(',', parameters)}): {Type} {{\n{base.ToString()}\n}}";
    }
}
