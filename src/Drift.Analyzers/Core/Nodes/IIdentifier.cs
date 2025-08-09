using System;
using Drift.Analyzers.Core.Location;

namespace Drift.Analyzers.Core.Nodes;

public interface IIdentifier
{
    string Identifier { get; }
    SourceLocation Location { get; }
}
