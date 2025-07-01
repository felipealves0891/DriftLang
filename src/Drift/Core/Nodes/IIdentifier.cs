using System;
using Drift.Core.Location;

namespace Drift.Core.Nodes;

public interface IIdentifier
{
    string Identifier { get; }
    SourceLocation Location { get; }
}
