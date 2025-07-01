using System;
using System.Collections.Concurrent;
using Drift.Core.Ast.Types;
using Drift.Core.Functions;

namespace Drift.Core;

public static class DriftEnv
{
    public static DataTypeRegistry TypeRegistry { get; } = new();
    public static BuiltInFunctionRegistry FunctionRegistry { get; } = new();
}
