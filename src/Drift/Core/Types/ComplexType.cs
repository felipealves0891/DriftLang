using System;
using Drift.Core.Ast.Types;

namespace Drift.Core.Types;

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
