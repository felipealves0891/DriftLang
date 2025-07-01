using System;
using Drift.Core.Nodes;

namespace Drift.Core.Helpers;

public class BreakFlow : Exception
{
    public IDriftValue Value { get; init; }

    public BreakFlow(IDriftValue value)
    {
        Value = value;
    }
}
