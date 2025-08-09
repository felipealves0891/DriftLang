using System;
using Drift.Analyzers.Core.Nodes;
using Drift.Analyzers.Lexer;

namespace Drift.Analyzers.Parser.NodeParser;

public interface INodeParser
{
    bool IsParse(TokenType before, TokenType current, TokenType next);
    DriftNode Parse(ITokenSource source, DriftNode? parent = null);
}
