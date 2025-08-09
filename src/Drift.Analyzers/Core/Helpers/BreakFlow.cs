using System;
using Drift.Analyzers.Core.Nodes;

namespace Drift.Analyzers.Core.Helpers;

public class BreakFlow : Exception
{
    public IDriftValue Value { get; init; }

    public BreakFlow(IDriftValue value)
    {
        Value = value;
    }
}
