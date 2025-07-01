using System;
using Drift.Core.Nodes;
using Drift.Lexer;

namespace Drift.Parser.NodeParser;

public interface INodeParser
{
    bool IsParse(TokenType before, TokenType current, TokenType next);
    DriftNode Parse(ITokenSource source, DriftNode? parent = null);
}
