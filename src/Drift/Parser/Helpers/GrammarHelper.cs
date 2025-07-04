using System;
using Drift.Core;
using Drift.Core.Ast.Types;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Declarations;
using Drift.Core.Nodes.Expressions;
using Drift.Core.Nodes.Statements;
using Drift.Core.Nodes.Values;
using Drift.Core.Types;
using Drift.Lexer;
using Drift.Parser.NodeParser;

namespace Drift.Parser.Helpers;

public static class GrammarHelper
{
    public static TryExpression TryExpressionParse(ITokenSource source)
    {
        if (!source.Match(TokenType.TRY))
            throw source.InvalidTokenException(TokenType.TRY, source.Current.Type);

        var start = source.Current.Location;
        source.Advance();
        var expression = ExpressionHelper.Parsing(source);

        if (!source.Match(TokenType.OPEN_BRACE))
            throw source.InvalidTokenException(TokenType.OPEN_BRACE, source.Current.Type);

        var actions = new Dictionary<TokenType, ActionStatement>();
        source.Advance([TokenType.SUCCESS, TokenType.ERROR]);

        while (source.Current.Type is not TokenType.CLOSE_BRACE and not TokenType.EOF)
        {
            var type = source.Current.Type;
            source.Advance(TokenType.OPEN_PAREN);
            var action = ActionParser(source);
            actions[type] = action;
            if(source.Match(TokenType.COMMA))
                source.Advance();
        }
        
        var end = source.Current.Location;
        source.Advance();

        return new TryExpression(
            expression,
            actions[TokenType.SUCCESS],
            actions[TokenType.ERROR],
            start.Join(end));
    }

    public static ActionStatement ActionParser(ITokenSource source)
    {
        var start = source.Current.Location;
        var parameters = ParametersParse(source);
        var type = TypeParser(source);
        source.Advance(TokenType.OPEN_BRACE);
        var block = source.BlockParse();
        source.Advance();
        var end = source.Current.Location;
        return new ActionStatement(parameters, type, block, start.Join(end));
    }

    public static StructInstanceValue StructInstanceValueParse(ITokenSource source)
    {
        if (!source.Match(TokenType.IDENTIFIER))
            throw source.InvalidTokenException(TokenType.IDENTIFIER, source.Current.Type);

        var typeName = source.Current.Source;
        source.Advance(TokenType.OPEN_BRACE);
        
        var start = source.Current.Location;
        source.Advance(TokenType.IDENTIFIER);
        var fields = new Dictionary<string, ExpressionNode>();
        
        while (source.Current.Type != TokenType.CLOSE_BRACE)
        {
            var identifier = source.Current;
            source.Advance(TokenType.ASSIGNMENT);
            source.Advance();
            var value = ExpressionHelper.Parsing(source);
            fields[identifier.Source] = value;

            if (source.Match(TokenType.COMMA))
                source.Advance();
        }

        var end = source.Current.Location;
        source.Advance();

        var type = DriftEnv.TypeRegistry.Resolve(typeName);
        return new StructInstanceValue(type, fields, start.Join(end));

    } 

    public static ExpressionNode[] ArgumentsParse(ITokenSource source)
    {
        if (!source.Match(TokenType.OPEN_PAREN))
            return [];

        var exprs = new List<ExpressionNode>();
        source.Advance();
        while (!source.Match(TokenType.CLOSE_PAREN))
        {
            var expr = ExpressionHelper.Parsing(source);
            exprs.Add(expr);
            
            if (source.Match(TokenType.COMMA))
                source.Advance();
        }

        source.Advance();
        return exprs.ToArray();
    }

    public static VariableDeclaration[] ParametersParse(ITokenSource source)
    {
        if (!source.Match(TokenType.OPEN_PAREN))
            return [];

        var parameters = new List<VariableDeclaration>();
        source.Advance(TokenType.IDENTIFIER);
        while (source.Current.Type != TokenType.CLOSE_PAREN)
        {
            var identifier = source.Current;
            source.Advance(TokenType.COLON);
            var type = TypeParser(source);
            var location = identifier.Location.Join(source.Current.Location);

            parameters.Add(new VariableDeclaration(identifier.Source, type, location));
            source.Advance([TokenType.COMMA, TokenType.CLOSE_PAREN]);
            if (source.Match(TokenType.CLOSE_PAREN))
                continue;

            source.Advance(TokenType.IDENTIFIER);
        }

        source.Advance();
        return parameters.ToArray();
    }

    public static IDataType TypeParser(ITokenSource source)
    {
        var registry = DriftEnv.TypeRegistry;

        source.Advance(TokenType.IDENTIFIER);
        var identifier = source.Current.Source;
        var type = registry.Resolve(identifier);

        if (source.Next.Type == TokenType.OPEN_BRACKET)
        {
            source.Advance(TokenType.OPEN_BRACKET);
            source.Advance(TokenType.CLOSE_BRACKET);
            identifier += "[]";
            if (!registry.Exists(identifier))
                registry.Register(new CompositeType(identifier, type));

            type = registry.Resolve(identifier);
        }

        return type;
    }

}
