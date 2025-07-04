using System;
using Drift.Core.Nodes;

namespace Drift.Core.Helpers;

public class ErrorFlow : BreakFlow
{
    public ErrorFlow(IDriftValue value) : base(value)
    {
    }
}
