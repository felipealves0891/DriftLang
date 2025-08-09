using System;

namespace Drift.VirtualMachine.Instructions;

public record Instruction(InstructionCode Code, object? Arg = null);
