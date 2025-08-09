using System;

namespace Drift.Analyzers.CodeGen;

public interface ICodeGen
{
    void GenerateCode(Stack<Instruction> instructions);
}
