using Drift.Analyzers.Core.Ast.Types;
using Drift.Analyzers.Core.Functions;

namespace Drift.Analyzers.Core;

public static class DriftEnv
{
    public static IStackFrame StackFrame { get; set; } 
    public static DataTypeRegistry TypeRegistry { get; } = new();
    public static BuiltInFunctionRegistry FunctionRegistry { get; } = new();
}
