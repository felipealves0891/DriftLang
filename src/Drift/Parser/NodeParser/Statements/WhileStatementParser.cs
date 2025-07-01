using System;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Statements;
using Drift.Lexer;
using Drift.Parser.Helpers;

namespace Drift.Parser.NodeParser.Statements;

public class WhileStatementParser : INodeParser
{
    public bool IsParse(TokenType before, TokenType current, TokenType next)
    {
        return current == TokenType.WHILE;
    }

    public DriftNode Parse(ITokenSource source, DriftNode? parent = null)
    {
        var start = source.Current.Location;
        source.Advance();
        var expression = ExpressionHelper.Parsing(source);
        var nodes = source.BlockParse();
        var end = source.Current.Location;
        source.Advance();
        return new WhileStatement(expression, nodes, start.Join(end));
    }
}
