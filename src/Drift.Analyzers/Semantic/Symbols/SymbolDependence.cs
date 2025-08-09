using Drift.Analyzers.Core.Ast.Types;
using Drift.Analyzers.Core.Location;

namespace Drift.Analyzers.Semantic.Symbols;

public struct SymbolDependence
{
    public string Identifier { get; }
    public IDataType Type { get; }
    public bool Immutable { get; } = true;
    public SourceLocation Location { get; }

    public SymbolDependence(
        string identifier,
        IDataType type,
        SourceLocation location)
    {
        Identifier = identifier;
        Type = type;
        Location = location;
    }

    public SymbolDependence(Symbol symbol)
    {
        Identifier = symbol.Identifier;
        Type = symbol.Type;
        Location = symbol.Location;
        Immutable = symbol.Immutable;
    }
}
