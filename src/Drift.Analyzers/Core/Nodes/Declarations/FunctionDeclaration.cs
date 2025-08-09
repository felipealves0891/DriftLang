using Drift.Analyzers.Core.Nodes.Statements;
using Drift.Analyzers.Core.Ast.Types;
using Drift.Analyzers.Core.Location;
using Drift.Analyzers.CodeGen;

namespace Drift.Analyzers.Core.Nodes.Declarations;

public class FunctionDeclaration : DeclarationNode
{
    public FunctionDeclaration(
        string identifier,
        VariableDeclaration[] parameters,
        IDataType type,
        StatementNode[] nodes,
        SourceLocation location)
        : base(identifier, nodes, location)
    {
        Parameters = parameters;
        Type = type;
        foreach (var parameter in Parameters)
            parameter.Parent = this;
    }

    public VariableDeclaration[] Parameters { get; }
    public IDataType Type { get; }
    public override DriftNode[] Children => Nodes;

    public override void Declare(IExecutionContext context)
    {
        context.Declare(Identifier);
    }

    public override void Execute(IExecutionContext context)
    {
        var function = context.CreateFunction(this);
        context.Set(Identifier, function);
    }

    public override void GenerateCode(Stack<Instruction> instructions)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        var parameters = string.Join(',', Parameters.Select(x => $"{x.Identifier}:{x.Type}"));
        return $"fun {Identifier} ({parameters}) {{...}}";
    }
}
