using System;
using Drift.Analyzers.Core.Nodes;

namespace Drift.Analyzers.Core.Helpers;

public class ErrorFlow : BreakFlow
{
    public ErrorFlow(IDriftValue value) : base(value)
    {
    }
}
