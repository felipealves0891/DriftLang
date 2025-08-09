using System;
using Drift.Analyzers.Core.Ast.Types;

namespace Drift.Analyzers.Core.Nodes;

public interface IDrift
{
    IDataType Type { get; }
}
