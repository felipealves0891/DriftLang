using System;
using Drift.Core;
using Drift.Core.Ast.Types;
using Drift.Core.Helpers;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Declarations;
using Drift.Core.Nodes.Statements;

namespace Drift.Runtime;

public class FunctionInterpreter : DriftInterpreter, IDriftFunction
{
    public FunctionInterpreter(IExecutionContext context, BlockStatement statement)
        : base(context, statement)
    {
        Name = "statement";
        Type = DriftEnv.TypeRegistry.Resolve("void");
        Parameters = new Dictionary<string, IDataType>();
    }

    public FunctionInterpreter(IExecutionContext context, FunctionDeclaration declaration)
        : base(context, declaration)
    {
        Name = declaration.Identifier;
        Type = declaration.Type;
        Parameters = new Dictionary<string, IDataType>();

        foreach (var parameter in declaration.Parameters)
            Parameters[parameter.Identifier] = parameter.Type;
    }

    public FunctionInterpreter(IExecutionContext context, OnDeclaration declaration)
        : base(context, declaration)
    {
        Name = declaration.Identifier;
        Type = declaration.Type;
        Parameters = new Dictionary<string, IDataType>();

        foreach (var parameter in declaration.Parameters)
            Parameters[parameter.Identifier] = parameter.Type;
    }

    public string Name { get; }
    public IDictionary<string, IDataType> Parameters { get; }
    public IDataType Type { get; }

    public IDriftValue? Invoke(IDictionary<string, IDriftValue> parameters)
    {
        Reset();
        using (Context.EnterScope())
        {
            foreach (var key in parameters.Keys)
            {
                Context.Declare(key);
                Context.Set(key, parameters[key]);
            }

            try
            {
                return InternalInterpret();    
            }
            catch (BreakFlow breakFlow)
            {
                return breakFlow.Value;
            }
            
        }
        
    }
}
