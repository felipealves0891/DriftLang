using System;
using Drift.Analyzers.Core.Nodes;
using Drift.Analyzers.Core.Nodes.Declarations;
using Drift.Analyzers.Core.Nodes.Statements;
using Drift.Analyzers.Lexer;

namespace Drift.Analyzers.Parser.NodeParser.Statements;

public class ExportStatementParser : INodeParser
{
    public bool IsParse(TokenType before, TokenType current, TokenType next)
    {
        return current == TokenType.EXPORT;
    }

    public DriftNode Parse(ITokenSource source, DriftNode? parent = null)
    {
        var start = source.Current.Location;
        source.Advance();
        var declaration = (FunctionDeclaration)source.NextNode();
        var end = source.Current.Location;
        return new ExportStatement(declaration, start.Join(end));
    }
}
