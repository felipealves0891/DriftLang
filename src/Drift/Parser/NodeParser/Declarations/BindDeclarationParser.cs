using System;
using Drift.Core;
using Drift.Core.Ast.Types;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Declarations;
using Drift.Core.Nodes.Expressions;
using Drift.Lexer;
using Drift.Parser.Helpers;

namespace Drift.Parser.NodeParser.Declarations;

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
