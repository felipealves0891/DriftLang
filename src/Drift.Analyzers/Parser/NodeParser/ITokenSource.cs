using System;
using Drift.Analyzers.Core.Nodes;
using Drift.Analyzers.Core.Nodes.Statements;
using Drift.Analyzers.Lexer;

namespace Drift.Analyzers.Parser.NodeParser;

public interface ITokenSource
{
    Token Before { get; }
    Token Current { get; }
    Token Next { get; }
    DriftNode NextNode(DriftNode? parent = null);
    void Advance();
    void Advance(TokenType expected);
    void Advance(TokenType[] expected);
    bool Match(TokenType expected);
    bool Match(TokenType[] expected);
    StatementNode[] BlockParse();
    Exception InvalidTokenException(TokenType expected, TokenType current);
    Exception InvalidTokenException(TokenType[] expected, TokenType current);
}
