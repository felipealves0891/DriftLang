namespace Dvm;

public record Instruction(InstructionCode Code, object? Arg = null);
