using System;
using System.Collections.Concurrent;
using Drift.Core.Helpers;
using Drift.Semantic.Diagnostic;

namespace Drift.Semantic.References;

public class ReferenceMapper
{
    private readonly ConcurrentDictionary<Reference, HashSet<Reference>> _references;
    private readonly ConcurrentStack<Reference> _from;
    private readonly DiagnosticAggregator _aggregator;

    public ReferenceMapper(DiagnosticAggregator aggregator)
    {
        _from = new();
        _references = new();
        _aggregator = aggregator;
    }

    public Reference? CurrentFrom =>
       _from.TryPeek(out var name) ? name : null!;

    public IDisposable EnterFrom(Reference from)
    {
        _from.Push(from);
        return new ExiterScope(ExitFrom);
    }

    public void ExitFrom()
    {
        if (!_from.TryPop(out var variable))
            return;

        var visited = new HashSet<Reference>(new ReferenceEqualityComparer());
        var stack = new HashSet<Reference>(new ReferenceEqualityComparer());

        var hasCycle = HasCycle(variable, visited, stack);
        if (hasCycle.Status)
        {
            var cycleStr = string.Join(" → ", hasCycle.CyclePath);
            _aggregator.AddFailureReactiveCycle(cycleStr, variable.Location);
        }

        _references.Clear();
    }

    private (bool Status, string[] CyclePath) HasCycle(Reference variable, HashSet<Reference> visited, HashSet<Reference> stack)
    {
        if (stack.Contains(variable))
        {
            var from = stack.First(x => x.Equals(variable));
            return (true, GetCyclePath(variable, stack));
        }

        if (visited.Contains(variable)) return (false, []);

        visited.Add(variable);
        stack.Add(variable);

        if (_references.TryGetValue(variable, out var deps))
        {
            foreach (var dep in deps)
            {
                var (status, cyclePath) = HasCycle(dep, visited, stack);
                if (status)
                    return (true, cyclePath);
            }
        }

        stack.Remove(variable);
        return (false, []);
    }

    private string[] GetCyclePath(Reference variable, HashSet<Reference> stack)
    {
        return stack.Reverse()
                    .Append(variable)
                    .Select(x => x.ToString())
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToArray();
    }
    
    public void SetTo(Reference to)
    {
        if(CurrentFrom is null)
            return;
            
        if(!_references.ContainsKey(CurrentFrom)) _references[CurrentFrom] = new HashSet<Reference>();
        _references[CurrentFrom].Add(to);
    }

}
