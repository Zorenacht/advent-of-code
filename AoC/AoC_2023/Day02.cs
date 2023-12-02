using FluentAssertions;
using MathNet.Numerics.Providers.LinearAlgebra;
using Tools;

namespace AoC_2023;

public sealed class Day02 : Day
{
    [Puzzle(answer: 2283)]
    public int Part1(string input)
    {
        int result = 0;
        var lines = input.Lines();
        foreach (var line in lines)
        {
            var split1 = line.Split(": ");
            int game = int.Parse(split1[0].Split("Game ")[1]);
            var draws = split1[1].Split("; ");
            var valid = true;
            foreach (var draw in draws)
            {
                var balls = draw.Split(", ");
                foreach (var ball in balls)
                {
                    var amount = int.Parse(ball.Split(" ")[0]);
                    var color = ball.Split(" ")[1];
                    if (color == "red" && amount > 12) valid = false;
                    if (color == "green" && amount > 13) valid = false;
                    if (color == "blue" && amount > 14) valid = false;
                }
            }
            if (valid) result += game;

        }
        return result;
    }

    [Puzzle(answer: 78669)]
    public int Part2(string input)
    {
        int result = 0;
        var lines = input.Lines();
        foreach (var line in lines)
        {
            var split1 = line.Split(": ");
            int game = int.Parse(split1[0].Split("Game ")[1]);
            var draws = split1[1].Split("; ");
            var valid = true;
            var min = new int[3] { 0, 0, 0 };
            foreach (var draw in draws)
            {
                var balls = draw.Split(", ");
                foreach (var ball in balls)
                {
                    var amount = int.Parse(ball.Split(" ")[0]);
                    var color = ball.Split(" ")[1];
                    if (color == "red") min[0] = Math.Max(min[0], amount);
                    if (color == "green") min[1] = Math.Max(min[1], amount);
                    if (color == "blue") min[2] = Math.Max(min[2], amount);
                }
            }
            if (valid) result += min[0] * min[1] * min[2];

        }
        return result;
    }
}