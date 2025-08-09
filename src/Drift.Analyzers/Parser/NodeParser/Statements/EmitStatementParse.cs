using System;
using Drift.Analyzers.Core.Nodes;
using Drift.Analyzers.Core.Nodes.Expressions;
using Drift.Analyzers.Core.Nodes.Statements;
using Drift.Analyzers.Lexer;
using Drift.Analyzers.Parser.Helpers;

namespace Drift.Analyzers.Parser.NodeParser.Statements;

public class EmitStatementParse : INodeParser
{
    public bool IsParse(TokenType before, TokenType current, TokenType next)
    {
        return current == TokenType.EMIT;
    }

    public DriftNode Parse(ITokenSource source, DriftNode? parent = null)
    {
        var start = source.Current.Location;
        source.Advance();
        var identifier = source.Current;
        source.Advance();
        var arguments = GrammarHelper.ArgumentsParse(source);
        var positions = new Dictionary<string, ExpressionNode>();
        
        for (int i = 0; i < arguments.Count(); i++)
            positions[i.ToString()] = arguments[i];
        
        if (!source.Match(TokenType.SEMICOLON))
            throw source.InvalidTokenException(TokenType.SEMICOLON, source.Current.Type);

        source.Advance();
        return new EmitStatement(identifier.Source, positions, start.Join(identifier.Location));
    }
}
