using System;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Expressions;
using Drift.Core.Nodes.Helpers;
using Drift.Core.Nodes.Values;
using Drift.Lexer;
using Drift.Lexer.Extensions;
using Drift.Parser.NodeParser;

namespace Drift.Parser.Helpers;

public class ExpressionHelper
{
    private readonly ITokenSource _source;

    public ExpressionHelper(ITokenSource source)
    {
        _source = source;
    }

    public TokenType BeforeType => _source.Before.Type;
    public string BeforeSource => _source.Before.Source;

    public TokenType CurrentType => _source.Current.Type;
    public string CurrentSource => _source.Current.Source;

    public TokenType NextType => _source.Next.Type;
    public string NextSource => _source.Next.Source;

    public DriftNode ParseExpression()
    {
        var left = ParseTerm();
        while (
            (CurrentType is TokenType.AND or TokenType.OR) ||
            (CurrentSource is "==" or "!=") ||
            (CurrentSource is ">" or "<") ||
            (CurrentSource is ">=" or "<=")
        )
        {
            var op = CurrentSource;
            Consume();
            var right = ParseTerm();
            var location = left.Location.Join(right.Location);
            left = new BinaryExpression(left, op, right, location);
        }
        return left;
    }

    public DriftNode ParseTerm()
    {
        var left = ParseFactor();
        while (CurrentSource is "+" or "-")
        {
            var op = CurrentSource;
            Consume();
            var right = ParseFactor();
            var location = left.Location.Join(right.Location);
            left = new BinaryExpression(left, op, right, location);
        }
        return left;
    }

    public DriftNode ParseFactor()
    {
        var left = ParseUnary();
        while (CurrentSource is "*" or "/")
        {
            var op = CurrentSource;
            Consume();
            var right = ParseUnary();
            var location = left.Location.Join(right.Location);
            left = new BinaryExpression(left, op, right, location);
        }
        return left;
    }

    public DriftNode ParseUnary()
    {
        var location = _source.Current.Location;
        var indicator = CurrentSource;
        if (indicator is "not" or "-")
        {
            Consume();
            var value = ToExpression(ParseValue());
            return new UnaryExpression(value, indicator, location.Join(value.Location));
        }
        else
        {
            return ParseValue();
        }
    }

    public DriftNode ParseValue()
    {
        if (CurrentType.IsLiteral())
            return LiteralParse();
        else if (CurrentType == TokenType.IDENTIFIER && NextType == TokenType.OPEN_PAREN)
            return FunctionCallExpressionParse();
        else if (CurrentType == TokenType.IDENTIFIER && NextType == TokenType.OPEN_BRACKET)
            return ArrayAccessExpressionParse();
        else if (CurrentType == TokenType.IDENTIFIER && NextType == TokenType.ARROW)
            return StructAccessParser();
        else if (BeforeType == TokenType.ASSIGNMENT && CurrentType == TokenType.IDENTIFIER && NextType == TokenType.OPEN_BRACE)
            return GrammarHelper.StructInstanceValueParse(_source);
        else if (CurrentType == TokenType.IDENTIFIER)
            return IdentifierParse();
        else
            return ParseComplexExpressions();
    }

    private DriftNode StructAccessParser()
    {
        var instance = _source.Current;
        Consume();
        var property = Consume();
        Consume();
        return new StructAccessExpression(
            instance.Source,
            property.Source,
            instance.Location.Join(property.Location));
    }

    private DriftNode LiteralParse()
    {
        var literal = _source.Current.ToLiteral();
        Consume();
        return literal;
    }

    private FunctionCallExpression FunctionCallExpressionParse()
    {
        var identifier = _source.Current;
        Consume();
        var arguments = GrammarHelper.ArgumentsParse(_source);
        return new FunctionCallExpression(identifier.Source, arguments, identifier.Location);
    }

    private ArrayAccessExpression ArrayAccessExpressionParse()
    {
        var identifier = _source.Current;
        _source.Advance(TokenType.OPEN_BRACKET);
        _source.Advance();
        var result = ToExpression(ParseExpression());

        Consume();
        return new ArrayAccessExpression(
            identifier.Source,
            result,
            identifier.Location.Join(result.Location));
    }

    private IdentifierNode IdentifierParse()
    {
        var identifier = new IdentifierNode(CurrentSource, _source.Current.Location);
        Consume();
        return identifier;
    }

    public DriftNode ParseComplexExpressions()
    {
        if (CurrentType == TokenType.OPEN_PAREN)
            return ParsePrefered();
        else if (CurrentType == TokenType.OPEN_BRACKET)
            return ParseArray();
        else if (CurrentType == TokenType.TRY)
            return GrammarHelper.TryExpressionParse(_source);

        throw new InvalidOperationException($"Token invalido para expressão: {CurrentType}");
    }

    private DriftNode ParsePrefered()
    {
        Consume();
        var value = ParseExpression();
        if (CurrentType != TokenType.CLOSE_PAREN)
            throw new InvalidOperationException($"Token invalido para expressão: {CurrentType} era esperado ')'");
        Consume();
        return value;
    }

    private ArrayExpression ParseArray()
    {
        var start = _source.Current.Location;
        var array = new List<ExpressionNode>();

        Consume();
        while (CurrentType != TokenType.CLOSE_BRACKET)
        {
            array.Add(ToExpression(ParseExpression()));
            if (CurrentType is TokenType.CLOSE_BRACKET) break;
            if (CurrentType is not TokenType.COMMA)
                throw new InvalidOperationException($"Token invalido para expressão: {CurrentType}");

            Consume();
        }

        var end = _source.Current.Location;
        Consume();
        return new ArrayExpression(array.ToArray(), start.Join(end));
    }

    private Token Consume()
    {
        _source.Advance();
        return _source.Current;
    }

    public static ExpressionNode Parsing(ITokenSource source)
    {
        var helper = new ExpressionHelper(source);
        return ToExpression(helper.ParseExpression());
    }

    private static ExpressionNode ToExpression(DriftNode result)
    { 
        return result is ExpressionNode expr
            ? expr
            : new ValueExpression((IDriftValue)result, result.Location);
    }

}
