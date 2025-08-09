using System;
using Drift.Analyzers.Core.Nodes.Declarations;
using Drift.Analyzers.Core.Nodes.Values;
using Drift.Analyzers.Core.Location;
using Drift.Analyzers.Core.Nodes.Expressions;
using Drift.Analyzers.CodeGen;

namespace Drift.Analyzers.Core.Nodes.Statements;

public class ForInStatement : BlockStatement
{
    public ForInStatement(
        VariableDeclaration declaration,
        ExpressionNode source,
        StatementNode[] nodes,
        SourceLocation location) : base(nodes, location)
    {
        Declaration = declaration;
        Declaration.Parent = this;
        Source = source;
        Source.Parent = this;
    }

    public VariableDeclaration Declaration { get; }
    public ExpressionNode Source { get; }

    public override DriftNode[] Children => Nodes;

    public override void Execute(IExecutionContext context)
    {
        throw new NotImplementedException();
    }

    public override void GenerateCode(Stack<Instruction> instructions)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return $"for {Declaration} in {Source} {{\n{base.ToString()}\n}}";
    }
}
