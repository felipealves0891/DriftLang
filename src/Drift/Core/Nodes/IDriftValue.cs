using System;
using Drift.Core.Ast.Types;

namespace Drift.Core.Nodes;

public interface IDriftValue : IDrift
{
    object Unwrap { get; }
}
