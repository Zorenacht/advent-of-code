using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System;

namespace AoC;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class PuzzleTypeAttribute(params PuzzleType[] types) : Attribute, IApplyToTest
{
    public PuzzleType[] Types { get; } = types;

    public void ApplyToTest(Test test)
    {
        foreach (var type in Types)
            test.Properties.Add("Type", type.ToString());
    }
}

public enum PuzzleType
{
    Compute,
    Ordering,
    Parsing,
    Grid,
    Cycle,
    DP,
    Recursion,
    ShortestPath,
    FloodFill,
    LinearSystem,
    Manual,
    Tree,
    Permutations,
}