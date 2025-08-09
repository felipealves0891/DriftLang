using System;
using Drift.Analyzers.Core.Nodes;
using Drift.Analyzers.Lexer;
using Drift.Analyzers.Parser.Helpers;

namespace Drift.Analyzers.Parser.NodeParser.Expressions;

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
