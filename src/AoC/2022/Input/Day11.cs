namespace AoC._2022.Input;

public sealed class Day11Input
{
    public static List<Day11.Monkey> ExampleInput => new()
    {
        new Day11.Monkey(
            new List<long>() { 79, 98 },
            old => old* 19,
            Day11.Monkey.CreateTest(23,2,3)
        ),
        new Day11.Monkey(
            new List<long>() { 54,65,75,74 },
            old => old + 6,
            Day11.Monkey.CreateTest(19,2,0)
        ),
        new Day11.Monkey(
            new List<long>() { 79,60,97 },
            old => old * old,
            Day11.Monkey.CreateTest(13,1,3)
        ),
        new Day11.Monkey(
            new List<long>() { 74 },
            old => old + 3,
            Day11.Monkey.CreateTest(17,0,1)
        ),
    };

    public static List<Day11.Monkey> Input => new()
    {
        new Day11.Monkey(
            new () { 53, 89, 62, 57, 74, 51, 83, 97 },
            old => old * 3,
            Day11.Monkey.CreateTest(13,1,5)
        ),
        new Day11.Monkey(
            new List<long>() { 85, 94, 97, 92, 56 },
            old => old + 2,
            Day11.Monkey.CreateTest(19,5,2)
        ),
        new Day11.Monkey(
            new List<long>() { 86, 82, 82 },
            old => old + 1,
            Day11.Monkey.CreateTest(11,3,4)
        ),
        new Day11.Monkey(
            new List<long>() { 94, 68},
            old => old + 5,
            Day11.Monkey.CreateTest(17,7,6)
        ),
        new Day11.Monkey(
            new List<long>() { 83, 62, 74, 58, 96, 68, 85 },
            old => old + 4,
            Day11.Monkey.CreateTest(3,3,6)
        ),
        new Day11.Monkey(
            new List<long>() { 50, 68, 95, 82 },
            old => old + 8,
            Day11.Monkey.CreateTest(7,2,4)
        ),
        new Day11.Monkey(
            new List<long>(){ 75 },
            old => old * 7,
            Day11.Monkey.CreateTest(5,7,0)
        ),
        new Day11.Monkey(
            new List<long>() { 92, 52, 85, 89, 68, 82 },
            old => old * old,
            Day11.Monkey.CreateTest(2,0,1)
        ),
    };
}