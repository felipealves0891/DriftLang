using System;
using Drift.Analyzers.Core.Location;
using Drift.Analyzers.Core.Nodes;
using Drift.Analyzers.Core.Nodes.Expressions;
using Drift.Analyzers.Core.Nodes.Statements;
using Drift.Analyzers.Lexer;
using Drift.Analyzers.Parser.NodeParser;
using Serilog;

namespace Drift.Analyzers.Parser;

public class DriftParser : ITokenSource
{
    private readonly ILogger _logger;
    private readonly IList<INodeParser> _parsers;
    private readonly Tokenizer _tokenizer;
    private Token _beforeToken;
    private Token _currentToken;
    private Token _nextToken;

    public DriftParser(Tokenizer tokenizer)
    {
        _logger = Log.ForContext<DriftParser>();
        _tokenizer = tokenizer;
        _currentToken = _tokenizer.NextToken();
        _nextToken = _tokenizer.NextToken();
        _parsers = typeof(INodeParser)
                    .Assembly
                    .GetTypes()
                    .Where(t => typeof(INodeParser).IsAssignableFrom(t) && t.IsClass)
                    .Select(t => Activator.CreateInstance(t))
                    .Cast<INodeParser>()
                    .ToList();
    }

    public Token Before => _beforeToken;
    public Token Current => _currentToken;
    public Token Next => _nextToken;

    public ScriptNode Parse()
    {
        var nodes = new List<StatementNode>();
        while (Current.Type != TokenType.EOF)
        {
            var node = NextNode();
            _logger.Debug(node.ToString() ?? node.GetType().Name);
            
            StatementNode statement = node is StatementNode stmt
                                    ? stmt
                                    : new ExpressionStatement((ExpressionNode)node, node.Location);

            nodes.Add(statement);
        }

        return new ScriptNode(nodes.ToArray(), new SourceLocation());
    }

    public DriftNode NextNode(DriftNode? parent = null)
    {
        foreach (var parser in _parsers)
        {
            if (parser.IsParse(Before.Type, Current.Type, Next.Type))
            {
                return parser.Parse(this, parent);
            }
        }

        throw new InvalidDataException($"Token Invalido: {_currentToken}");
    }

    public void Advance()
    {
        _beforeToken = _currentToken;
        _currentToken = _nextToken;
        _nextToken = _tokenizer.NextToken();
    }

    public void Advance(TokenType expected)
    {
        Advance();
        if (!Match(expected))
            throw InvalidTokenException(expected, _currentToken.Type);
    }

    public bool Match(TokenType type)
    {
        return _currentToken.Type == type;
    }

    public void Advance(TokenType[] expected)
    {
        Advance();
        if (!Match(expected))
            throw InvalidTokenException(expected, _currentToken.Type);
    }

    public bool Match(TokenType[] expected)
    {
        return expected.Contains(_currentToken.Type);
    }

    public StatementNode[] BlockParse()
    {
        if (_currentToken.Type != TokenType.OPEN_BRACE)
            return [];

        var statements = new List<StatementNode>();
        Advance();
        while (_currentToken.Type is not TokenType.CLOSE_BRACE and not TokenType.EOF)
        {
            var node = NextNode();
            StatementNode statement = node is StatementNode stmt
                                    ? stmt
                                    : new ExpressionStatement((ExpressionNode)node, node.Location);

            statements.Add(statement);
        }

        return statements.ToArray();
    }

    public Exception InvalidTokenException(TokenType expected, TokenType current)
    {
        throw new InvalidOperationException($"Token invalido, esperado {expected} e recebido {current}!");
    }
    
    public Exception InvalidTokenException(TokenType[] expected, TokenType current)
    { 
        throw new InvalidOperationException($"Token invalido, esperado {string.Join(" ou ", expected)} e recebido {current}!");
    }
}
