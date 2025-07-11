using System;
using Drift.Core.Ast.Types;
using Drift.Core.Location;
using Drift.Core.Nodes.Declarations;

namespace Drift.Semantic.Symbols;

public struct Symbol
{
    public string Identifier { get; }
    public IDataType Type { get; }
    public SourceLocation Location { get; }
    public bool Immutable { get; } = true;
    public List<SymbolDependence> Dependences { get; } = new();

    public Symbol(
        string identifier,
        IDataType type,
        SourceLocation location)
        : this()
    {
        Identifier = identifier;
        Type = type;
        Location = location;
    }

    public Symbol(
        string identifier,
        bool immutable,
        IDataType type,
        SourceLocation location)
        : this()
    {
        Identifier = identifier;
        Type = type;
        Location = location;
        Immutable = immutable;
    }
}
