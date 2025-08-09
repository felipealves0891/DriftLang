using Drift.Analyzers.Core.Ast.Types;

namespace Drift.Analyzers.Core.Nodes;

public interface IDriftFunction : IDrift
{
    string Name { get; }
    IDictionary<string, IDataType> Parameters { get; }
    IDriftValue? Invoke(IDictionary<string, IDriftValue> parameters);
}