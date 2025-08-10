using System;

namespace Dvm.Instructions;

public record Operation(OperationCode Code, object? Arg = null);