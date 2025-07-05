using System;
using System.Globalization;
using Drift.Core.Location;
using Drift.Lexer.Reader;
using Serilog;

namespace Drift.Lexer;

public class Tokenizer
{
    private const byte SINGLE_QUOTES = (byte)'\'';
    private const byte OPEN_PARENTHESIS = (byte)'(';
    private const byte CLOSE_PARENTHESIS = (byte)')';
    private const byte OPEN_BRACE = (byte)'{';
    private const byte CLOSE_BRACE = (byte)'}';
    private const byte OPEN_BRACKET = (byte)'[';
    private const byte CLOSE_BRACKET = (byte)']';
    private const byte COMMA = (byte)',';
    private const byte SEMICOLON = (byte)';';
    private const byte ASSIGNMENT = (byte)'=';
    private const byte EXCLAMATION = (byte)'!';
    private const byte MINUS = (byte)'-';
    private const byte PLUS = (byte)'+';
    private const byte GREATER_THAN = (byte)'>';
    private const byte LESS_THAN = (byte)'<';
    private const byte ASTERISK = (byte)'*';
    private const byte SLASH = (byte)'/';
    private const byte DOT = (byte)'.';
    private const byte COLON = (byte)':';
    private const byte PIPE = (byte)'|';
    private const byte HASHTAG = (byte)'#';
    private const byte NEWLINE = (byte)'\n';
    private const byte UNDERSCORE = (byte)'_';
    private const byte AT = (byte)'@';

    private readonly IReader _source;
    private readonly ILogger _logger;

    public Tokenizer(IReader source)
    {
        _source = source;
        _logger = Log.ForContext<Tokenizer>();
    }

    public IEnumerable<Token> Tokenize() 
    {
        Token token;
        do
        {   
            token = NextToken();
            yield return token;
        } 
        while(token.Type != TokenType.EOF);
    }

    public Token NextToken() 
    {
        if(_source.EndOfFile)
            return new Token(TokenType.EOF, (byte)' ', _source.FileName, _source.Position, _source.Line);

        while(_source.CanNext && char.IsWhiteSpace((char)_source.CurrentChar)) 
            _source.Advance();

        if(_source.EndOfFile)
            return new Token(TokenType.EOF, (byte)' ', _source.FileName, _source.Position, _source.Line);

        if(char.IsLetter((char)_source.CurrentChar) || _source.CurrentChar == UNDERSCORE) 
            return GetIdentifierOrKeyword();
        if(char.IsDigit((char)_source.CurrentChar)) 
            return GetDigit();
        if(_source.CurrentChar == SINGLE_QUOTES) 
            return GetString();
        if(_source.CurrentChar == OPEN_PARENTHESIS) 
            return AdvanceAndReturn(TokenType.OPEN_PAREN, OPEN_PARENTHESIS);   
        if(_source.CurrentChar == CLOSE_PARENTHESIS) 
            return AdvanceAndReturn(TokenType.CLOSE_PAREN, CLOSE_PARENTHESIS);
        if(_source.CurrentChar == COMMA) 
            return AdvanceAndReturn(TokenType.COMMA, COMMA);          
        if(_source.CurrentChar == SEMICOLON) 
            return AdvanceAndReturn(TokenType.SEMICOLON, SEMICOLON);   
        if(IsAssignment(_source.BeforeChar, _source.CurrentChar, _source.NextChar)) 
            return AdvanceAndReturn(TokenType.ASSIGNMENT, ASSIGNMENT);   
        if(_source.BeforeChar == ASSIGNMENT && _source.CurrentChar == ASSIGNMENT) 
            return AdvanceAndReturn(TokenType.OPERATOR, ASSIGNMENT, ASSIGNMENT);   
        if(_source.CurrentChar == EXCLAMATION && _source.NextChar == ASSIGNMENT) 
            return AdvanceAndReturn(TokenType.OPERATOR, EXCLAMATION, ASSIGNMENT);   
        if(_source.CurrentChar == MINUS && _source.NextChar == GREATER_THAN)
            return GetArrow();
        if(_source.CurrentChar == LESS_THAN || _source.CurrentChar == GREATER_THAN) 
            return GetComparison();
        if(_source.CurrentChar == PLUS || _source.CurrentChar == MINUS) 
            return AdvanceAndReturn(TokenType.OPERATOR, _source.CurrentChar);   
        if(_source.CurrentChar == ASTERISK || _source.CurrentChar == SLASH)
            return AdvanceAndReturn(TokenType.OPERATOR, _source.CurrentChar);   
        if(_source.CurrentChar == EXCLAMATION)
            return AdvanceAndReturn(TokenType.OPERATOR, EXCLAMATION); 
        if(_source.CurrentChar == OPEN_BRACE)
            return AdvanceAndReturn(TokenType.OPEN_BRACE, _source.CurrentChar); 
        if(_source.CurrentChar == CLOSE_BRACE)
            return AdvanceAndReturn(TokenType.CLOSE_BRACE, _source.CurrentChar);
        if(_source.CurrentChar == DOT)
            return AdvanceAndReturn(TokenType.ACCESS, _source.CurrentChar);
        if(_source.CurrentChar == COLON)
            return AdvanceAndReturn(TokenType.COLON, _source.CurrentChar);
        if(_source.CurrentChar == PIPE)
            return AdvanceAndReturn(TokenType.PIPE, _source.CurrentChar);
        if(_source.CurrentChar == OPEN_BRACKET)
            return AdvanceAndReturn(TokenType.OPEN_BRACKET, _source.CurrentChar);
        if(_source.CurrentChar == CLOSE_BRACKET)
            return AdvanceAndReturn(TokenType.CLOSE_BRACKET, _source.CurrentChar);
        if(_source.CurrentChar == AT)
            return AdvanceAndReturn(TokenType.AT, _source.CurrentChar);
        if (_source.CurrentChar == HASHTAG)
            return Comment();
        if(_source.CurrentChar == NEWLINE && !_source.CanNext)
        {
            _source.Advance();
            return NextToken();
        }
        
        if(!_source.EndOfFile)
            _source.Advance();

        return NextToken();
    }

    private Token Comment() 
    {
        _source.Advance();
        while (!_source.EndOfFile && _source.CurrentChar != NEWLINE)
            _source.Advance();
        
        if(!_source.EndOfFile)
            _source.Advance();

        return NextToken();
    }
    
    private Token AdvanceAndReturn(TokenType type, params byte[] source) 
    {
        var token = new Token(type, source, _source.FileName, _source.Column, _source.Line);
        _source.Advance();
        _logger.Debug(token.ToString());
        return token;
    }
    
    public Token GetDigit() 
    {
        var start = _source.Position;
        Position positionStart = new (_source.Line, _source.Column);
        bool isDot = false;
        int positionDot = -1;
        while (!_source.EndOfFile && (char.IsDigit((char)_source.CurrentChar) || _source.CurrentChar == DOT))
        {
            if (_source.CurrentChar == DOT)
            {
                positionDot = _source.Position - start;
                isDot = _source.CurrentChar == DOT;

            }
            _source.Advance();
        }

        var number = _source.GetString(start, _source.Position);
        if (isDot) number[positionDot] = DecimalSeparator();

        var positionEnd = new Position(_source.Line, _source.Column);
        var token = isDot
            ? new Token(TokenType.FLOAT_LITERAL, number, _source.FileName, positionStart, positionEnd)
            : new Token(TokenType.INTEGER_LITERAL, number, _source.FileName, positionStart, positionEnd);

        _logger.Debug(token.ToString());
        return token;
    }

    private byte DecimalSeparator()
    { 
        return (byte)CultureInfo.CurrentCulture
            .NumberFormat.NumberDecimalSeparator
            .ToArray()
            .Single();
    }

    private Token GetIdentifierOrKeyword()
    {
        var start = _source.Position;
        Position positionStart = new(_source.Line, _source.Column);

        while (!_source.EndOfFile && IsIdentifier(_source.CurrentChar))
            _source.Advance();

        var identifier = _source.GetString(start, _source.Position);
        var type = Keywords.IsKeyword(identifier);
        var positionEnd = new Position(_source.Line, _source.Column);
        var token = new Token(type, identifier, _source.FileName, positionStart, positionEnd);
        _logger.Debug(token.ToString());
        return token;
    }

    private Token GetString() 
    {
        _source.Advance();
        var start = _source.Position;
        Position positionStart = new (_source.Line, _source.Column);

        while (!_source.EndOfFile && _source.CurrentChar != SINGLE_QUOTES)
            _source.Advance();
        
        var end = _source.Position;
        var positionEnd = new Position(_source.Line, _source.Column);
        var text = _source.GetString(start, end);
        _source.Advance();

        var token = new Token(TokenType.STRING_LITERAL, text, _source.FileName, positionStart, positionEnd);
        _logger.Debug(token.ToString());
        return token;
    }

    private Token GetComparison() 
    {
        var start = _source.Position;
        Position positionStart = new (_source.Line, _source.Column);
        Position positionEnd;

        _source.Advance();
        if(_source.BeforeChar != ASSIGNMENT)
        {
            positionEnd = new Position(_source.Line, _source.Column);
            return new Token(TokenType.OPERATOR, _source.BeforeChar, _source.FileName, positionStart, positionEnd);
        }
        
        var end = _source.Position;        
        positionEnd = new Position(_source.Line, _source.Column);
        _source.Advance();

        var token = new Token(TokenType.OPERATOR, _source.GetString(start, end), _source.FileName, positionStart, positionEnd);
        _logger.Debug(token.ToString());
        return token;
    }

    private Token GetArrow() 
    {
        var start = new Position(_source.Line, _source.Column);
        _source.Advance();
        _source.Advance();
        var end = new Position(_source.Line, _source.Column);
        var token = new Token(TokenType.ARROW, [MINUS, GREATER_THAN], _source.FileName, start, end);
        _logger.Debug(token.ToString());
        return token;
    }

    
    
    public bool IsAssignment(byte beforeChar, byte currentChar, byte nextChar)
        => IsLetterOrIsDigitOrWhiteSpace(beforeChar) && currentChar == ASSIGNMENT && IsLetterOrIsDigitOrWhiteSpace(nextChar);

    public bool IsIdentifier(byte b) 
        => char.IsLetter((char)b) || char.IsDigit((char)b) || (char)b == UNDERSCORE;

    private bool IsLetterOrIsDigitOrWhiteSpace(byte c) 
        => char.IsLetter((char)c) || char.IsDigit((char)c) || char.IsWhiteSpace((char)c);
}
