using System;

namespace Drift.Analyzers.Lexer.Extensions;

public static class TokenTypeExtensions
{
    public static bool IsLiteral(this TokenType type)
    {
        return type switch
        {
            TokenType.STRING_LITERAL => true,
            TokenType.FLOAT_LITERAL => true,
            TokenType.BOOLEAN_LITERAL => true,
            TokenType.INTEGER_LITERAL => true,
            TokenType.TRUE => true,
            TokenType.FALSE => true,
            _ => false
        };
    }
}
