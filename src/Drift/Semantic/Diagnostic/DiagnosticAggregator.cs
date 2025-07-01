using System;
using System.Text;
using Drift.Core.Location;

namespace Drift.Semantic.Diagnostic;

public class DiagnosticAggregator
{
    private readonly HashSet<DiagnosticMessage> _messages;

    public DiagnosticAggregator()
    {
        _messages = new HashSet<DiagnosticMessage>();
    }

    public DiagnosticAggregator(IEnumerable<DiagnosticMessage> initialMessages)
    {
        _messages = new(initialMessages);
    }

    public void AddErrorWasNotDeclared(string identifier, SourceLocation location)
        => AddError($"The {identifier} symbol was not declared", location);
        
    public void AddErrorAlreadyDeclaration(string identifier, SourceLocation location)
        => AddError($"The {identifier} symbol has already been declared", location);

    public void AddErrorIncompatibleTypes(string lType, string rType, SourceLocation location)
        => AddError($"Expected type {lType} but is being assigned {rType}", location);

    public void AddFailureReactiveCycle(string identifier, SourceLocation location)
        => AddError($"Reactive cycle detected involving variable {identifier}", location);

    public void AddError(string message, SourceLocation location)
        => _messages.Add(new DiagnosticMessage(DiagnosticSeverity.Error, message, location));

    public void AddWarning(string message, SourceLocation location)
        => _messages.Add(new DiagnosticMessage(DiagnosticSeverity.Warning, message, location));

    public void AddSuggestion(string message, SourceLocation location)
        => _messages.Add(new DiagnosticMessage(DiagnosticSeverity.Suggestion, message, location));

    public void AddMessage(DiagnosticMessage message)
        => _messages.Add(message);

    public IReadOnlyCollection<DiagnosticMessage> All => _messages;

    public IEnumerable<DiagnosticMessage> Errors =>
        _messages.Where(m => m.Severity == DiagnosticSeverity.Error);

    public IEnumerable<DiagnosticMessage> Warnings =>
        _messages.Where(m => m.Severity == DiagnosticSeverity.Warning);

    public IEnumerable<DiagnosticMessage> Suggestions =>
        _messages.Where(m => m.Severity == DiagnosticSeverity.Suggestion);

    public bool HasErrors => Errors.Any();

    public void MergeWith(DiagnosticAggregator other)
    {
        foreach (var message in other._messages)
            _messages.Add(message);
    }

    public override string ToString()
    {
        var builder = new StringBuilder();

        foreach (var severity in Enum.GetValues<DiagnosticSeverity>())
        {
            var entries = _messages.Where(m => m.Severity == severity).OrderBy(m => m.Location.Start.Line).ToList();
            if (entries.Count == 0) continue;

            builder.AppendLine($"--- {severity.ToString().ToUpper()}S ({entries.Count}) ---");
            foreach (var entry in entries)
                builder.AppendLine(entry.ToString());
            builder.AppendLine();
        }

        return builder.ToString();
    }
}
