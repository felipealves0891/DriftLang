using System;
using System.Text.RegularExpressions;
using Drift.Analyzers.Core.Types;

namespace Drift.Analyzers.Core.Ast.Types;

public class NativeType : IDataType, IEquatable<IDataType>
{
    private readonly Regex? _regex;

    public NativeType(string name, Type native)
    {
        Name = name;
        Native = native;
    }

    public NativeType(string name, Type native, string regex)
    {
        Name = name;
        Native = native;
        _regex = new Regex(regex);
    }

    public string Name { get; init; }
    public Type Native { get; init; }

    public bool Equals(IDataType? other)
    {
        if (other is null)
            return false;

        if (_regex is null)
            return base.Equals(other) || other.Name == TypeNames.Any;
        else
            return _regex.IsMatch(other.Name);
    }
    
    public override bool Equals(object? obj)
        => Equals(obj as IDataType);

    public override int GetHashCode()
        => base.GetHashCode();

    public override string ToString()
    {
        return Name;
    }
}
