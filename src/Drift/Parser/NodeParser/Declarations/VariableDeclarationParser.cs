using System;
using Drift.Core.Location;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Declarations;
using Drift.Core.Nodes.Expressions;
using Drift.Lexer;
using Drift.Parser.Helpers;

namespace Drift.Parser.NodeParser.Declarations;

public class VariableDeclarationParser : INodeParser
{
    public bool IsParse(TokenType before, TokenType current, TokenType next)
    {
        return current == TokenType.LET
            || current == TokenType.CONST;
    }

    public DriftNode Parse(ITokenSource source, DriftNode? parent = null)
    {
        var start = source.Current.Location;

        var isAsync = source.Current.Type == TokenType.ASYNC;
        var isImmutable = source.Current.Type == TokenType.CONST;

        if (isAsync) source.Advance(TokenType.LET);
        source.Advance(TokenType.IDENTIFIER);

        var identifier = source.Current.Source;
        source.Advance(TokenType.COLON);

        var type = GrammarHelper.TypeParser(source);
        var location = start.Join(source.Current.Location);
        source.Advance();

        if (source.Match(TokenType.ASSIGNMENT))
        {
            source.Advance();
            var result = ExpressionHelper.Parsing(source);
            var assignment = result is ExpressionNode expr
                        ? expr
                        : new ValueExpression((IDriftValue)result, result.Location);

            if (source.Match(TokenType.SEMICOLON))
                source.Advance();

            return new VariableDeclaration(
                identifier, type, isImmutable, isAsync, assignment, location);
        }

        if(source.Match(TokenType.SEMICOLON))
            source.Advance();
            
        return new VariableDeclaration(
            identifier, type, isImmutable, isAsync, location);
    

        
    }
    
}
