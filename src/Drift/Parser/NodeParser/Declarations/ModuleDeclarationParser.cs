using System;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Declarations;
using Drift.Lexer;

namespace Drift.Parser.NodeParser.Declarations;

public class ModuleDeclarationParser : INodeParser
{
    public bool IsParse(TokenType before, TokenType current, TokenType next)
    {
        return current == TokenType.MODULE;
    }

    public DriftNode Parse(ITokenSource source, DriftNode? parent = null)
    {
        var start = source.Current.Location;

        source.Advance(TokenType.IDENTIFIER);
        var identifier = source.Current.Source;
        source.Advance(TokenType.OPEN_BRACE);

        var block = source.BlockParse();
        var end = source.Current.Location;
        source.Advance();
        return new ModuleDeclaration(identifier, block, start.Join(end));
    }
}
