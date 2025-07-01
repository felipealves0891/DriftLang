using Drift.Core.Ast.Types;

namespace Drift.Core.Nodes;

public interface IDriftFunction : IDrift
{
    string Name { get; }
    IDictionary<string, IDataType> Parameters { get; }
    IDriftValue? Invoke(IDictionary<string, IDriftValue> parameters);
}