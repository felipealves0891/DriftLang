using System;
using Drift.Analyzers.Core.Ast.Types;
using Drift.Analyzers.Core.Nodes;

namespace Drift.Analyzers.Core.Functions;

public class BuiltInFunction : IDriftFunction
{
    private readonly Func<IDictionary<string, IDriftValue>, IDriftValue?> _implementation;

    public BuiltInFunction(
        string name,
        IDictionary<string, string> parameters,
        string returnType,
        Func<IDictionary<string, IDriftValue>, IDriftValue?> implementation)
    {
        Name = name;
        Parameters = new Dictionary<string, IDataType>();
        foreach (var parameter in parameters)
            Parameters[parameter.Key] = DriftEnv.TypeRegistry.Resolve(parameter.Value);

        Type = DriftEnv.TypeRegistry.Resolve(returnType);
        _implementation = implementation;
    }

    public BuiltInFunction(
        string name,
        IDictionary<string, IDataType> parameters,
        IDataType returnType,
        Func<IDictionary<string, IDriftValue>, IDriftValue?> implementation)
    {
        Name = name;
        Parameters = parameters;
        Type = returnType;
        _implementation = implementation;
    }

    public string Name { get; init; }
    public IDictionary<string, IDataType> Parameters { get; init; }
    public IDataType Type { get; init; }

    public IDriftValue? Invoke(IDictionary<string, IDriftValue> parameters)
        => _implementation(parameters);
}
