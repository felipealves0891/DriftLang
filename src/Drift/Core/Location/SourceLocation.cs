using System;
using System.Diagnostics.CodeAnalysis;

namespace Drift.Core.Location;

public struct SourceLocation
{
    public SourceLocation(string file, Position start, Position end)
    {
        File = file;
        Start = start;
        End = end;
    }

    public string File { get; init; }
    public Position Start { get; init; }
    public Position End { get; init; }

    public override string ToString()
    {
        return $"{File} from {Start} to {End}";
    }

    public SourceLocation Join(SourceLocation end)
    {
        return new SourceLocation(
            File,
            Start,
            end.End);
    }
}
