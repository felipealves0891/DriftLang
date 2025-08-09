using System;
using Drift.Analyzers.Core.Ast.Types;

namespace Drift.Analyzers.Core.Types;

public class ComplexType : IDataType
{
    public string Name { get; init; }
    public IDictionary<string, IDataType> Properties { get; }

    public ComplexType(
        string name,
        IDictionary<string, IDataType> properties)
    {
        Name = name;
        Properties = properties;
    }

    public override string ToString()
    {
        return Name;
    }
}
