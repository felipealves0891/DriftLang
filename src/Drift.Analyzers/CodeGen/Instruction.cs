using System;

namespace Drift.Analyzers.CodeGen;

public class Instruction
{
    public Instruction(
        Instruction op,
        object? arg1 = null,
        object? arg2 = null,
        object? arg3 = null)
    {
        Op = op;
        Arg1 = arg1;
        Arg2 = arg2;
        Arg3 = arg3;
    }

    public Instruction Op { get; private set; }
    public object? Arg1 { get; private set; }
    public object? Arg2 { get; private set; }
    public object? Arg3 { get; private set; }
}
