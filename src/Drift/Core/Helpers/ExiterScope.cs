using System;

namespace Drift.Core.Helpers;

public class ExiterScope : IDisposable
{
    private readonly Action _action;
    private bool _disposed;

    public ExiterScope(Action action)
    {
        _action = action;
        _disposed = false;
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _action();
    }
}
