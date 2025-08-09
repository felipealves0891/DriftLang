using System;
using System.Text.RegularExpressions;
using Drift.Analyzers.Core.Ast.Types;

namespace Drift.Analyzers.Core.Types;

public class CompositeType : IDataType, IEquatable<IDataType>
{
    private readonly Regex? _regex;

    public CompositeType(
        string name,
        IDataType compose)
    {
        Name = name;
        Compose = compose;
    }

    public CompositeType(
        string name,
        IDataType compose,
        string regex)
    {
        Name = name;
        Compose = compose;
        _regex = new Regex(regex);
    }

    public string Name { get; init; }
    public IDataType Compose { get; }

    public bool Equals(IDataType? other)
    {
        if (other is null)
            return false;

        if (_regex is null)
            return base.Equals(other) || other.Name == TypeNames.ArrayOfAny;
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
