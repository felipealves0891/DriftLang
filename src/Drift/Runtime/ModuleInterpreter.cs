using System;
using Drift.Core;
using Drift.Core.Ast.Types;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Statements;

namespace Drift.Runtime;

public class ModuleInterpreter : DriftInterpreter, IDriftModule
{
    public ModuleInterpreter(
        string name,
        IExecutionContext context,
        BlockStatement block)
        : base(context, block)
    {
        Name = name;
        Type = DriftEnv.TypeRegistry.Resolve(name);
    }

    public string Name { get; }
    public IDataType Type { get; }

    public void Initialize()
    {
        InternalInterpret();
    }

    public IDriftValue? Invoke(string name, IDictionary<string, IDriftValue> parameters)
    {
        if (!Context.Exposed(name))
            throw new InvalidOperationException($"the identifier {name} does not exist or is not public");

        var function = (IDriftFunction)Context.Get(name);
        return function.Invoke(parameters);
    }
}
