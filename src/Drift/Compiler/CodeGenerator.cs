using System;
using Drift.Analyzers.Core.Nodes;
using Drift.Analyzers.Core.Nodes.Declarations;
using Drift.Analyzers.Core.Nodes.Expressions;
using Drift.Analyzers.Core.Nodes.Helpers;
using Drift.Analyzers.Core.Nodes.Literals;
using Drift.Analyzers.Core.Nodes.Statements;
using Drift.Analyzers.Core.Nodes.Values;
using Dvm.Instructions;

namespace Drift.Compiler;

public class CodeGenerator
{
    private readonly Stack<Operation> _operations;

    public CodeGenerator()
    {
        _operations = new();
    }

    public IEnumerable<Operation> Generate(ScriptNode node)
    {
        _operations.Clear();
        Parse(node);
        return _operations.Reverse().ToArray();
    }

    private void Parse(DriftNode node)
    {
        foreach (var child in node.Children)
            Parse(child);

        if (node is DeclarationNode declaration) { }
        else if (node is ExpressionNode expression) { }
        else if (node is IdentifierNode identifier) { }
        else if (node is LiteralNode literal) { }
        else if (node is StatementNode statement) { }
        else if (node is ValueNode valueNode) { }
        
    }

    private void DeclarationParse(DeclarationNode declaration)
    {
        if (declaration is BindDeclaration bind) { }
        else if (declaration is EventDeclaration evn) { }
        else if (declaration is FunctionDeclaration func) { }
        else if (declaration is OnDeclaration on) { }
        else if (declaration is StructDeclaration obj) { }
        else if (declaration is StructFieldDeclaration objField) { }
        else if (declaration is VariableDeclaration var) {}
    }
}
