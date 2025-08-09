using System;
using Drift.Analyzers.Core;
using Drift.Analyzers.Core.Ast.Types;
using Drift.Analyzers.Core.Nodes;
using Drift.Analyzers.Core.Nodes.Declarations;
using Drift.Analyzers.Core.Nodes.Expressions;
using Drift.Analyzers.Lexer;
using Drift.Analyzers.Parser.Helpers;

namespace Drift.Analyzers.Parser.NodeParser.Declarations;

public class BindDeclarationParser : INodeParser
{
    public bool IsParse(TokenType before, TokenType current, TokenType next)
    {
        return current == TokenType.BIND;
    }

    public DriftNode Parse(ITokenSource source, DriftNode? parent = null)
    {
        var start = source.Current.Location;
        source.Advance(TokenType.IDENTIFIER);
        var identifier = source.Current;

        source.Advance(TokenType.COLON);
        source.Advance(TokenType.IDENTIFIER);
        var type = DriftEnv.TypeRegistry
                           .Resolve(source.Current.Source);

        source.Advance(TokenType.ARROW);
        source.Advance();

        var expression = ExpressionHelper.Parsing(source);

        source.Advance();
        return new BindDeclaration(
            identifier.Source,
            type,
            expression,
            start.Join(expression.Location));
    }
}
