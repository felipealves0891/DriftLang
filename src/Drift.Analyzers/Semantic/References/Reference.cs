using System;
using Drift.Analyzers.Core.Location;
using Drift.Analyzers.Core.Nodes.Declarations;
using Drift.Analyzers.Core.Nodes.Expressions;
using Drift.Analyzers.Core.Nodes.Helpers;

namespace Drift.Analyzers.Semantic.References;

public class Reference
{
    public Reference(string identifier, Type type, SourceLocation location)
    {
        Identifier = identifier;
        Type = type;
        Location = location;
    }

    public Reference(BindDeclaration bind)
        : this(bind.Identifier, bind.GetType(), bind.Location)
    { }

    public Reference(OnDeclaration bind)
        : this(bind.Identifier, bind.GetType(), bind.Location)
    { }

    public Reference(IdentifierNode node)
        : this(node.Value, node.GetType(), node.Location)
    { }

    public string Identifier { get; }
    public Type Type { get; }
    public SourceLocation Location { get; }

    public override bool Equals(object? obj)
    {
        return obj is Reference reference
            && reference.Identifier == Identifier;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Identifier);
    }

    public override string ToString()
    {
        return $"({Type.Name}:{Location.Start.Line}){Identifier}";
    }
}
