using System;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Statements;
using Drift.Lexer;

namespace Drift.Parser.NodeParser;

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
