using System.Collections.Concurrent;
using System.Collections.Immutable;
using Drift.Analyzers.Core.Location;
using Drift.Analyzers.Core.Nodes.Literals;
using Drift.Analyzers.Core.Nodes.Values;

namespace Drift.Analyzers.Core.Functions;

public class BuiltInFunctionRegistry
{
    private readonly ConcurrentDictionary<string, BuiltInFunction> _registries;

    public BuiltInFunctionRegistry()
    {
        _registries = new ConcurrentDictionary<string, BuiltInFunction>();
        Register(new BuiltInFunction(
            name: "println",
            parameters: new Dictionary<string, string>{
                { "format", "string" },
                { "args", "any[]" }
            },
            returnType: "string",
            implementation: args =>
            {
                var format = args.TryGetValue("format", out var outFormat) ? ((StringLiteral)outFormat).Value : "";
                var data = args.TryGetValue("args", out var outVars) ? ((ArrayValue)outVars).Source.Select(x => x.Unwrap).ToArray() : [];
                var message = string.Format(format, data);
                Console.WriteLine(message);
                return new StringLiteral(message, new SourceLocation());
            }
        ));
    }

    public IEnumerable<string> RegisteredFunctions
        => _registries.Keys.ToImmutableArray();

    public BuiltInFunction Resolve(string name)
    {
        return _registries[name];
    }

    public BuiltInFunction Register(BuiltInFunction data)
    {
        return _registries[data.Name] = data;
    }

    public bool Exists(string name)
    {
        return _registries.ContainsKey(name);
    }
    
}
