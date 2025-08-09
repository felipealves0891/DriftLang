using System;
using Drift.Analyzers.Core.Nodes;

namespace Drift.Analyzers.Core;

public interface IStackFrame : IEnumerable<DriftNode>
{
    IDisposable Push(DriftNode node);
    DriftNode? Pop();
}
