using System;
using Drift.Analyzers.Core.Ast.Types;
using Drift.Analyzers.Core.Nodes;
using Drift.Analyzers.Core.Nodes.Declarations;
using Drift.Analyzers.Lexer;
using Drift.Analyzers.Parser.Helpers;

namespace Drift.Analyzers.Parser.NodeParser.Declarations;

public class EventDeclarationParser : INodeParser
{
    public bool IsParse(TokenType before, TokenType current, TokenType next)
    {
        return current == TokenType.EVENT;
    }

    public DriftNode Parse(ITokenSource source, DriftNode? parent = null)
    {
        var start = source.Current;
        source.Advance(TokenType.IDENTIFIER);
        var identifier = source.Current;
        source.Advance(TokenType.OPEN_PAREN);
        var parameters = GrammarHelper.ParametersParse(source);
        var location = start.Location.Join(identifier.Location);
        source.Advance();
        return new EventDeclaration(identifier.Source, parameters, location);
    }
}
