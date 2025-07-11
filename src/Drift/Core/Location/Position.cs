using System;

namespace Drift.Core.Location;

public struct Position
{
    public Position(int line, int column)
    {
        Line = line;
        Column = column;
    }

    public int Line { get; }
    public int Column { get; }

    public override string ToString()
    {
        return $"({Line}:{Column})";
    }
}
