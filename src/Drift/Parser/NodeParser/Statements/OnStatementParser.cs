using System;
using Drift.Core;
using Drift.Core.Ast.Types;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Declarations;
using Drift.Lexer;
using Drift.Parser.Helpers;

namespace Drift.Parser.NodeParser.Statements;

public class OnDeclarationParser : INodeParser
{
    public bool IsParse(TokenType before, TokenType current, TokenType next)
    {
        return current == TokenType.ON;
    }

    public DriftNode Parse(ITokenSource source, DriftNode? parent = null)
    {
        var start = source.Current.Location;
        source.Advance(TokenType.IDENTIFIER);
        var identifier = source.Current;
        source.Advance(TokenType.OPEN_PAREN);
        var parameters = GrammarHelper.ParametersParse(source);
        var block = source.BlockParse();
        source.Advance();
        var end = source.Current.Location;
        return new OnDeclaration(
            identifier.Source,
            parameters,
            DriftEnv.TypeRegistry.Resolve("void"),
            block,
            start.Join(end));
    }
}
