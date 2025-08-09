using System;
using System.Collections;
using Drift.Analyzers.Core.Nodes;
using Drift.Analyzers.Core.Nodes.Declarations;
using Drift.Analyzers.Core.Nodes.Expressions;
using Drift.Analyzers.Core.Nodes.Statements;
using Drift.Analyzers.Core.Nodes.Values;
using Drift.Analyzers.Lexer;
using Drift.Analyzers.Lexer.Extensions;
using Drift.Analyzers.Parser.Helpers;

namespace Drift.Analyzers.Parser.NodeParser.Statements;

public class ForStatementParser : INodeParser
{
    public bool IsParse(TokenType before, TokenType current, TokenType next)
    {
        return current == TokenType.FOR;
    }

    public DriftNode Parse(ITokenSource source, DriftNode? parent = null)
    {
        var start = source.Current.Location;
        source.Advance(TokenType.LET);
        var declaration = (VariableDeclaration)source.NextNode();

        if (!source.Match([TokenType.TO, TokenType.IN]))
            source.InvalidTokenException([TokenType.TO, TokenType.IN], source.Current.Type);

        if (source.Current.Type == TokenType.TO)
        {
            source.Advance();
            var until = ExpressionHelper.Parsing(source);
            var end = source.Current.Location;
            var block = source.BlockParse();

            source.Advance();
            return new ForToStatement(declaration, until, block, start.Join(end));
        }
        else
        {
            source.Advance();
            var value = ExpressionHelper.Parsing(source);
            var end = source.Current.Location;
            var block = source.BlockParse();

            source.Advance();
            return new ForInStatement(declaration, value, block, start.Join(end));
        }
        
    }
}
