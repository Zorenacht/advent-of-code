﻿using System.Diagnostics.CodeAnalysis;

namespace Tools.Geometry;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Rider")]
public enum Direction
{
    E = 0,
    NE = 1,
    N = 2,
    NW = 3,
    W = 4,
    SW = 5,
    S = 6,
    SE = 7,

    None = 100,
}
