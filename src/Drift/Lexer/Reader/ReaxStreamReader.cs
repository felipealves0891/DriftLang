using System;
using System.Text;

namespace Drift.Lexer.Reader;

public sealed class DriftStreamReader : IReader
{
    private readonly string _filename;
    private readonly FileStream _stream;

    public DriftStreamReader(string filename)
    {
        _filename = filename;
        _stream = File.OpenRead(filename);
    }

    public bool EndOfFile => _stream.CanRead && _stream.Position >= _stream.Length;

    public byte BeforeChar 
    {
        get
        {
            if (_stream.Position == 0)
                return (byte)' ';
                
            _stream.Position--;
            var b = _stream.ReadByte();
            return (byte)b;
        }
    }

    public byte CurrentChar 
    {
        get
        {
            var b = _stream.ReadByte();
            _stream.Position--;
            return (byte)b;
        }
    }

    public bool CanNext => _stream.Position + 1 > _stream.Length;

    public byte NextChar 
    {
        get
        {
            _stream.Position++;
            if(EndOfFile)
                return (byte)' ';
                
            var b = _stream.ReadByte();
            _stream.Position--;
            _stream.Position--;
            return (byte)b;
        }
    }

    public int Position => (int)_stream.Position;

    public string FileName => _filename;

    public int Line { get; private set; } = 1;

    public int Column { get; private set; } = 1;

    public void Advance()
    {
        if(EndOfFile)
            throw new InvalidOperationException("Não é possivel avançar após o fim do arquivo");

        Column++;
        if(CurrentChar == '\n')
        {
            Line++;
            Column = 0;
        } 
        
        _stream.Position++;
    }

    public void Dispose()
    {
        _stream.Dispose();
    }

    public byte[] GetString(int start, int end)
    {
        var currentChar = _stream.Position;
        _stream.Position = start;

        var chars = new byte[end - start];
        for (int i = 0; i < end - start; i++)
            chars[i] = (byte)_stream.ReadByte();
        
        _stream.Position = currentChar;
        return chars;

    }
}
