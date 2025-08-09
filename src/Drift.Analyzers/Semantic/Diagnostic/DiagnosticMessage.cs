using System;
using Drift.Analyzers.Core.Location;

namespace Drift.Analyzers.Semantic.Diagnostic;

public class DiagnosticMessage
{
    public DiagnosticSeverity Severity { get; }
    public string Message { get; }
    public SourceLocation Location { get; }

    public DiagnosticMessage(DiagnosticSeverity severity, string message, SourceLocation location)
    {
        Severity = severity;
        Message = message;
        Location = location;
    }

    public override string ToString() =>
        $"{Severity.ToString().ToUpper()}: {Message} ({Location})";

    public override bool Equals(object? obj)
    {
        if (obj is not DiagnosticMessage other) return false;
        return Severity == other.Severity &&
               Message == other.Message &&
               Location.Equals(other.Location);
    }

    public override int GetHashCode() =>
        HashCode.Combine(Severity, Message, Location);
}
