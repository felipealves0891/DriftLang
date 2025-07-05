using System;
using System.Collections;
using System.Collections.Concurrent;
using Drift.Core;
using Drift.Core.Helpers;
using Drift.Core.Nodes;
using Serilog;

namespace Drift.Runtime.StackFrame;

public static class DriftStackFrame
{
    public static IStackFrame StackFrame => _stackFrameContext.Value;
    private static readonly Lazy<InternalDriftStackFrame> _stackFrameContext =
        new(() => new InternalDriftStackFrame());

    private class InternalDriftStackFrame : IStackFrame
    {
        private readonly ILogger _logger;
        private readonly ConcurrentStack<DriftNode> _stack;

        public InternalDriftStackFrame()
        {
            _stack = new ConcurrentStack<DriftNode>();
            _logger = Log.ForContext<InternalDriftStackFrame>();
        }

        public IDisposable Push(DriftNode node)
        {
            _logger.Debug("> Push {0}", node);
            _stack.Push(node);
            return new ExiterScope(() => Pop());
        }

        public DriftNode? Pop()
        {
            var nodePop = _stack.TryPop(out var node) ? node : null; ;
            _logger.Debug("< Pop {0}", nodePop);
            return nodePop;
        }

        public IEnumerator<DriftNode> GetEnumerator()
        {
            return _stack.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
