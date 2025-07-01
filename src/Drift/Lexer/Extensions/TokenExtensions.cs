using System;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Literals;

namespace Drift.Lexer.Extensions;

public static class TokenExtensions
{
    public static DriftNode ToLiteral(this Token token)
    {
        return token.Type switch
        {
            TokenType.BOOLEAN_LITERAL
            or TokenType.TRUE
            or TokenType.FALSE
                => new BooleanLiteral(bool.Parse(token.Source), token.Location),
            TokenType.FLOAT_LITERAL
                => new FloatLiteral(token.Source, token.Location),
            TokenType.STRING_LITERAL
                => new StringLiteral(token.Source, token.Location),
            TokenType.INTEGER_LITERAL
                => new IntegerLiteral(token.Source, token.Location),
            _ => throw new InvalidCastException($"Token não é um literal: {token}")
        };
    }
}
