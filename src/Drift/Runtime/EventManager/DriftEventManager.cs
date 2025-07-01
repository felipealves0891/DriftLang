using System;
using System.Collections.Concurrent;
using Drift.Core.Nodes;

namespace Drift.Runtime.EventManager;

public class DriftEventManager
{
    private readonly ConcurrentDictionary<string, IList<IDriftFunction>> _events;

    public DriftEventManager()
    {
        _events = new();
    }

    public void Event(string name)
    {
        if (_events.ContainsKey(name))
            throw new KeyNotFoundException($"Event '{name}' is already declared.");

        _events[name] = new List<IDriftFunction>();
    }

    public void Publish(string name, IDictionary<string, IDriftValue> arguments)
    {
        if (!_events.ContainsKey(name))
            throw new KeyNotFoundException($"Event '{name}' is not declared.");

        var subscribes = _events[name];
        foreach (var subscribe in subscribes)
            subscribe.Invoke(arguments);
    }

    public void Subscribe(string name, IDriftFunction function)
    {
        if (!_events.ContainsKey(name))
            throw new KeyNotFoundException($"Event '{name}' is not declared.");

        _events[name].Add(function);
    }

}
