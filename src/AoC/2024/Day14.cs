using FluentAssertions;
using Tools.Geometry;
using Tools;
using MathNet.Numerics;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;

namespace AoC_2024;

public sealed class Day14 : Day
{
    private class Robot(Index2D start, Index2D velocity)
    {
        public Index2D Start { get; } = start;
        public Index2D Velocity { get; } = velocity;
        public Index2D Current { get; set; } = start;

        public override bool Equals(object? obj) => obj is Robot other && Current.Equals(other.Current);
        public override int GetHashCode() => Current.GetHashCode();
    };

    [Puzzle(answer: 218619120)]
    public long Part1()
    {
        int result = 0;
        int height = 103;
        int width = 101;
        var lines = Input;
        var grid = new Grid<int>(height, width);
        //int height = 7;
        //int width = 11;
        //var lines = InputExample;
        //var grid = new Grid<int>(height, width);
        var robots = new List<Robot>();
        foreach (var line in lines)
        {
            var splitted = line.Split("= ,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var index = new Index2D(int.Parse(splitted[2]), int.Parse(splitted[1]));
            var velocity = new Index2D(int.Parse(splitted[5]), int.Parse(splitted[4]));
            robots.Add(new Robot(index, velocity));
            //grid[index.Row][index.Col] += 1;
        }
        for (int i = 0; i < 100; ++i)
        {
            foreach (var robot in robots)
            {
                robot.Current = new Index2D(
                    (robot.Current.Row + robot.Velocity.Row).Modulo(height),
                    (robot.Current.Col + robot.Velocity.Col).Modulo(width));
            }
        }
        var quadrants = new long[4] { 0, 0, 0, 0 };
        foreach (var robot in robots)
        {
            grid.UpdateAt(robot.Current, grid.ValueAt(robot.Current) + 1);
            if (robot.Current.Row < height / 2 && robot.Current.Col < width / 2) quadrants[0]++;
            if (robot.Current.Row < height / 2 && robot.Current.Col > width / 2) quadrants[1]++;
            if (robot.Current.Row > height / 2 && robot.Current.Col < width / 2) quadrants[2]++;
            if (robot.Current.Row > height / 2 && robot.Current.Col > width / 2) quadrants[3]++;
        }
        grid.Print();
        return quadrants[0] * quadrants[1] * quadrants[2] * quadrants[3];
    }


    [Puzzle(answer: 7055)]
    public long Part2()
    {
        int height = 103;
        int width = 101;
        var lines = Input;
        var robots = new List<Robot>();
        foreach (var line in lines)
        {
            var splitted = line.Split("= ,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var index = new Index2D(int.Parse(splitted[2]), int.Parse(splitted[1]));
            var velocity = new Index2D(int.Parse(splitted[5]), int.Parse(splitted[4]));
            var robot = new Robot(index, velocity);
            robots.Add(robot);
        }
        var biggestArea = new List<int>() { -100 };
        for (int i = 1; i <= 101 * 103; ++i)
        {
            var positions = new HashSet<Index2D>();
            foreach (var robot in robots)
            {
                robot.Current = new Index2D(
                    (robot.Current.Row + robot.Velocity.Row).Modulo(height),
                    (robot.Current.Col + robot.Velocity.Col).Modulo(width));
                positions.Add(robot.Current);
            }

            var areas = new HashSet<Area>();
            var visited = new HashSet<Index2D>();
            foreach (var robot in robots)
            {
                if (visited.Contains(robot.Current)) continue;
                var area = new Area();
                var queue = new Queue<Index2D>();
                queue.Enqueue(robot.Current);
                while (queue.TryDequeue(out var current))
                {
                    if (!visited.Add(current)) continue;
                    foreach (var dir in Directions.CardinalIndex)
                    {
                        var nb = current + dir;
                        if (positions.Contains(nb) && !visited.Contains(nb))
                        {
                            queue.Enqueue(nb);
                        }
                    }
                    area.Add(current);
                }
                areas.Add(area);
            }
            biggestArea.Add(areas.Max(x => x.Count));
        }
        return biggestArea.IndexOf(biggestArea.Max());
    }
};