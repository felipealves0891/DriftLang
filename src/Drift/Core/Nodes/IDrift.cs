using System;
using Drift.Core.Ast.Types;

namespace Drift.Core.Nodes;

public interface IDrift
{
    IDataType Type { get; }
}
