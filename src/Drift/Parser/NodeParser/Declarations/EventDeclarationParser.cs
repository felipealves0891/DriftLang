using System;
using Drift.Core.Ast.Types;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Declarations;
using Drift.Lexer;
using Drift.Parser.Helpers;

namespace Drift.Parser.NodeParser.Declarations;

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
