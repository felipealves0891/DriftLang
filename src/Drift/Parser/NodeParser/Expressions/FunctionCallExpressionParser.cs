using System;
using Drift.Core.Nodes;
using Drift.Lexer;
using Drift.Parser.Helpers;

namespace Drift.Parser.NodeParser.Expressions;

public class FunctionCallExpressionParser : INodeParser
{
    public bool IsParse(TokenType before, TokenType current, TokenType next)
    {
        return current == TokenType.IDENTIFIER && next == TokenType.OPEN_PAREN;
    }

    public DriftNode Parse(ITokenSource source, DriftNode? parent = null)
    {
        var functionCallExpression = ExpressionHelper.Parsing(source);
        source.Advance();
        return functionCallExpression;
    }
}
