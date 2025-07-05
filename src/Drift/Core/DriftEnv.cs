using Drift.Core.Ast.Types;
using Drift.Core.Functions;
using Drift.Runtime.StackFrame;

namespace Drift.Core;

public static class DriftEnv
{
    public static IStackFrame StackFrame { get; } = DriftStackFrame.StackFrame;
    public static DataTypeRegistry TypeRegistry { get; } = new();
    public static BuiltInFunctionRegistry FunctionRegistry { get; } = new();
}
