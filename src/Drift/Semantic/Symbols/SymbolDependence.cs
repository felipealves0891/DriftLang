using Drift.Core.Ast.Types;
using Drift.Core.Location;

namespace Drift.Semantic.Symbols;

public struct SymbolDependence
{
    public string Identifier { get; }
    public IDataType Type { get; }
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
    }
}
