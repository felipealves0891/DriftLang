using System;
using Drift.Analyzers.CodeGen;
using Drift.Analyzers.Core.Location;

namespace Drift.Analyzers.Core.Nodes;

public abstract class DriftNode : ICodeGen
{
    protected DriftNode(SourceLocation location)
    {
        Location = location;
    }

    public DriftNode? Parent { get; set; }
    public abstract DriftNode[] Children { get; }
    public SourceLocation Location { get; }

    public abstract void GenerateCode(Stack<Instruction> instructions);
}
