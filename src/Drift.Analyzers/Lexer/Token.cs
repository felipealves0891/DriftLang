using System;
using System.Text;
using Drift.Analyzers.Core.Location;

namespace Drift.Analyzers.Lexer;

public struct Token
{
    public Token()
    {
        Type = TokenType.EOF;
        Source = string.Empty;
    }

    public Token(TokenType type, string source, string file, Position start, Position end)
    {
        Source = source;
        Location = new SourceLocation(file, start, end);
        Type = type;
    }


    public Token(TokenType type, byte[] source, string file, Position start, Position end)
    {
        Source = Encoding.UTF8.GetString(source);
        Location = new SourceLocation(file, start, end);
        Type = type;
    }

    public Token(TokenType type, byte source, string file, Position start, Position end)
    {
        Source = Encoding.UTF8.GetString([source]);
        Location = new SourceLocation(file, start, end);
        Type = type;
    }
    
    public Token(TokenType type, byte source, string file, int column, int line)
    {
        Source = Encoding.UTF8.GetString([source]);
        Location = new SourceLocation(file, new Position(line, column), new Position(line, column));
        Type = type;
    }
    
    public Token(TokenType type, byte[] source, string file, int column, int line)
    {
        Source = Encoding.UTF8.GetString(source);;
        Location = new SourceLocation(file, new Position(line, column), new Position(line, column));
        Type = type;
    }

    public SourceLocation Location { get; init; }
    public string Source { get; init; }
    public TokenType Type { get; init; }
    


    public override string ToString()
    {
        return $"{Location} @ {Type} # {Source}";
    }
}
