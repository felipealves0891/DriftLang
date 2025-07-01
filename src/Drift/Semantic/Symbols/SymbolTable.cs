using System;
using System.Collections.Concurrent;
using Drift.Core.Helpers;

namespace Drift.Semantic.Symbols;

public class SymbolTable
{
    private readonly ConcurrentStack<ConcurrentDictionary<string, Symbol>> _scopes = new();
    public SymbolTable()
        => EnterScope();

    private ConcurrentDictionary<string, Symbol> CurrentScope
        => _scopes.TryPeek(out var table) ? table : new ConcurrentDictionary<string, Symbol>();
        
    public IDisposable EnterScope()
    {
        _scopes.Push(new());
        return new ExiterScope(ExitScope);

    }
    public void ExitScope()
    {
        _scopes.TryPop(out var _);
    }
    
    public bool Declare(Symbol symbol)
    {
        var current = CurrentScope;
        if (current.ContainsKey(symbol.Identifier)) return false;
        current[symbol.Identifier] = symbol;
        return true;
    }

    public Symbol? Resolve(string name)
    {
        foreach (var scope in _scopes)
            if (scope.TryGetValue(name, out var sym))
                return sym;

        return null;
    }

    public IEnumerable<SymbolDependence> ResolveParameters(string name)
    {
        var symbol = Resolve(name);
        if (!symbol.HasValue)
            return Enumerable.Empty<SymbolDependence>();

        return symbol.Value.Dependences;
    }
}
