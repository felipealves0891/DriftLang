using System;
using Drift.Core.Location;

namespace Drift.Core.Nodes;

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
