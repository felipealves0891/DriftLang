using System;
using Drift.Analyzers.Core;
using Drift.Analyzers.Core.Ast.Types;
using Drift.Analyzers.Core.Nodes;
using Drift.Analyzers.Core.Nodes.Declarations;
using Drift.Analyzers.Lexer;
using Drift.Analyzers.Parser.Helpers;

namespace Drift.Analyzers.Parser.NodeParser.Statements;

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
