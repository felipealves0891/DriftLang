using System;
using Drift.Analyzers.Core.Location;

namespace Drift.Analyzers.Core.Nodes;

public abstract class DriftNode
{
    protected DriftNode(SourceLocation location)
    {
        Location = location;
    }

    public DriftNode? Parent { get; set; }
    public abstract DriftNode[] Children { get; }
    public SourceLocation Location { get; }
    
}
