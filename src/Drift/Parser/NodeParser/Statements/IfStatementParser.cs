using System;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Expressions;
using Drift.Core.Nodes.Statements;
using Drift.Lexer;
using Drift.Parser.Helpers;

namespace Drift.Parser.NodeParser.Statements;

public class IfStatementParser : INodeParser
{
    public bool IsParse(TokenType before, TokenType current, TokenType next)
    {
        return current == TokenType.IF;
    }

    public DriftNode Parse(ITokenSource source, DriftNode? parent = null)
    {
        var start = source.Current.Location;
        source.Advance();
        var expr = ExpressionHelper.Parsing(source);
        var trueBlock = source.BlockParse();
        source.Advance();

        if (source.Match(TokenType.ELSE))
        {
            source.Advance();
            var falseBlock = source.BlockParse();
            source.Advance();
            return new IfStatement(
                expr, trueBlock, falseBlock, start.Join(source.Current.Location));
        }

        return new IfStatement(
                expr, trueBlock, start.Join(source.Current.Location));
    }
}
