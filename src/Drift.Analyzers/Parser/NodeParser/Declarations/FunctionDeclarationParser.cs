using System;
using Drift.Analyzers.Core.Nodes;
using Drift.Analyzers.Core.Nodes.Declarations;
using Drift.Analyzers.Lexer;
using Drift.Analyzers.Parser.Helpers;

namespace Drift.Analyzers.Parser.NodeParser.Declarations;

public class FunctionDeclarationParser : INodeParser
{
    public bool IsParse(TokenType before, TokenType current, TokenType next)
    {
        return current == TokenType.FUNCTION;
    }

    public DriftNode Parse(ITokenSource source, DriftNode? parent = null)
    {
        var start = source.Current.Location;
        source.Advance(TokenType.IDENTIFIER);
        var identifier = source.Current.Source;

        source.Advance(TokenType.OPEN_PAREN);
        var parameters = GrammarHelper.ParametersParse(source);

        var type = GrammarHelper.TypeParser(source);
        var end = source.Current.Location;

        source.Advance(TokenType.OPEN_BRACE);
        var block = source.BlockParse();
        source.Advance();
        
        return new FunctionDeclaration(
            identifier,
            parameters,
            type,
            block,
            start.Join(end));

    }
}
