using System;

namespace Drift.Analyzers.Core.Nodes;

public interface IDriftModule : IDrift
{
    string Name { get; }
    void Initialize();
    IDriftValue? Invoke(string name, IDictionary<string, IDriftValue> parameters);
}
