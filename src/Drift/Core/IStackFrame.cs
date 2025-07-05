using System;
using Drift.Core.Nodes;

namespace Drift.Core;

public interface IStackFrame : IEnumerable<DriftNode>
{
    IDisposable Push(DriftNode node);
    DriftNode? Pop();
}
