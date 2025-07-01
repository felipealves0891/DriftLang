using System.Collections.Concurrent;
using Drift.Core.Types;

namespace Drift.Core.Ast.Types;

public class DataTypeRegistry
{
    private readonly ConcurrentDictionary<string, IDataType> _registries;

    public DataTypeRegistry()
    {
        _registries = new ConcurrentDictionary<string, IDataType>();
        Register(new NativeType(TypeNames.Void, typeof(void)));
        Register(new NativeType(TypeNames.String, typeof(string)));
        Register(new NativeType(TypeNames.Integer, typeof(int)));
        Register(new NativeType(TypeNames.Float, typeof(float)));
        Register(new NativeType(TypeNames.Boolean, typeof(bool)));
        Register(new NativeType(TypeNames.Any, typeof(object), @"(\w*)$"));
        Register(new CompositeType(TypeNames.ArrayOfString, Resolve(TypeNames.String)));
        Register(new CompositeType(TypeNames.ArrayOfInteger, Resolve(TypeNames.Integer)));
        Register(new CompositeType(TypeNames.ArrayOfFloat, Resolve(TypeNames.Float)));
        Register(new CompositeType(TypeNames.ArrayOfBoolean, Resolve(TypeNames.Boolean)));
        Register(new CompositeType(TypeNames.ArrayOfAny, Resolve(TypeNames.Any), @"(\w*\[\])$"));
    }

    public IDataType Resolve(string name)
    {
        if (_registries.TryGetValue(name, out var type))
            return type;
        else
            return _registries["void"];
    }

    public IDataType Register(IDataType data)
    {
        return _registries[data.Name] = data;
    }

    public bool Exists(string name)
    {
        return _registries.ContainsKey(name);
    }
}
