using System;
using System.Collections.Concurrent;
using Drift.Core.Nodes;

namespace Drift.Runtime.EventManager;

public class DriftEventManager : IDisposable
{
    private readonly ConcurrentDictionary<string, IList<IDriftFunction>> _events;
    private readonly ConcurrentQueue<EventPublished> _queuePublished;
    private readonly Thread _queueProcess;
    private bool _shouldRun = true;

    public DriftEventManager()
    {
        _events = new();
        _queuePublished = new();
        _queueProcess = new Thread(new ThreadStart(QueueProcessor));
        _queueProcess.IsBackground = true;
        _queueProcess.Start();
    }

    private void QueueProcessor()
    {
        while (_shouldRun)
        {
            while (_queuePublished.TryDequeue(out var published))
            {
                if (_events.TryGetValue(published.Name, out var subscribes))
                {
                    foreach (var subscribe in subscribes)
                        subscribe.Invoke(published.Arguments);
                }
            }
        }
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

        _queuePublished.Enqueue(new EventPublished(name, arguments));
    }

    public void Subscribe(string name, IDriftFunction function)
    {
        if (!_events.ContainsKey(name))
            throw new KeyNotFoundException($"Event '{name}' is not declared.");

        _events[name].Add(function);
    }

    public void Dispose()
    {
        _shouldRun = false;
        _queueProcess.Join();
    }

    private class EventPublished
    {
        public EventPublished(
            string name,
            IDictionary<string, IDriftValue> arguments)
        {
            Name = name;
            Arguments = arguments;
        }

        public string Name { get; private set; }
        public IDictionary<string, IDriftValue> Arguments { get; private set; }
    }

}
