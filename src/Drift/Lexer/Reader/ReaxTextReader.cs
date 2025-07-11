using System;

namespace Drift.Lexer.Reader;

public class DriftTextReader : IReader
{
    private readonly string _source;
    private readonly string _filename;
    private int _position;

    public DriftTextReader(string source, string filename)
    {
        _source = source;
        _filename = filename;
        _position = 0;
    }

    public bool EndOfFile => Position >= _source.Length;
    public byte BeforeChar => Position > 0 ? (byte)_source[Position-1] : (byte)' ';
    public byte CurrentChar => (byte)_source[Position];
    public bool CanNext => Position + 1 < _source.Length;
    public byte NextChar => Position + 1 < _source.Length ? (byte)_source[Position+1] : (byte)' ';
    public int Position => _position;
    public string FileName => _filename;
    public int Line { get; private set; } = 1;
    public int Column { get; private set; } = 1;

    public void Advance() 
    {
        if(EndOfFile)
            throw new InvalidOperationException("Não é possivel avançar após o fim do arquivo!");

        Column++;
        if(CurrentChar == '\n')
        {
            Line++;
            Column = 0;
        }
        
        _position++;
    }

    public void Dispose()
    {
    }

    public byte[] GetString(int start, int end)
    {
        return _source[start..end].ToCharArray().Select(x => (byte)x).ToArray();
    }
}
