namespace AoC._2024;

[PuzzleType(PuzzleType.Grid, PuzzleType.FloodFill)]
public sealed class Day12 : Day
{
    [Puzzle(answer: 1370100)]
    public int Part1()
        => Input.ToCharGrid()
            .FloodFillRegions()
            .KeyedAreas.Values
            .Sum(x => x.Count * x.Border.Count);

    [Puzzle(answer: 818286)]
    public int Part2()
    {
        return Input.ToCharGrid()
            .FloodFillRegions()
            .KeyedAreas.Values
            .Sum(x => x.Count * x.NumberOfSides());
    }
};