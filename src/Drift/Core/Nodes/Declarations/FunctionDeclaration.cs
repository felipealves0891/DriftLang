using Drift.Core.Nodes.Statements;
using Drift.Core.Ast.Types;
using Drift.Core.Location;

namespace Drift.Core.Nodes.Declarations;

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
        foreach (var parameter in Parameters)
            parameter.Parent = this;

        Type = type;
        foreach (var node in Nodes)
            node.Parent = this;
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
        var function = context.CreateInterpreter(this);
        context.Set(Identifier, function);
    }

    public override string ToString()
    {
        var parameters = string.Join(',', Parameters.Select(x => $"{x.Identifier}:{x.Type}"));
        var block = string.Join('\n', Nodes.Select(x => $"\t{x}"));
        return $"fun {Identifier} ({parameters}) {{\n{block}\n}}";
    }
}
