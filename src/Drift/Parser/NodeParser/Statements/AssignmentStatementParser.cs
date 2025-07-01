using System;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Statements;
using Drift.Lexer;
using Drift.Parser.Helpers;

namespace Drift.Parser.NodeParser.Statements;

public class AssignmentStatementParser : INodeParser
{
    public bool IsParse(TokenType before, TokenType current, TokenType next)
    {
        return current == TokenType.IDENTIFIER
            && next == TokenType.ASSIGNMENT;
    }

    public DriftNode Parse(ITokenSource source, DriftNode? parent = null)
    {
        var start = source.Current.Location;
        var identifier = source.Current;
        source.Advance(TokenType.ASSIGNMENT);
        source.Advance();
        var expression = ExpressionHelper.Parsing(source);
        var end = source.Current.Location;
        source.Advance();
        return new AssignmentStatement(
            identifier.Source,
            expression,
            start.Join(end));
    }
}
