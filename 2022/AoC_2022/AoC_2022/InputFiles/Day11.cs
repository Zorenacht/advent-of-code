namespace AoC_2022;

public sealed partial class Day11
{
    private static List<Monkey> ExampleInput => new()
    {
        new Monkey(
            new List<long>() { 79, 98 },
            old => old* 19,
            Monkey.CreateTest(23,2,3)
        ),
        new Monkey(
            new List<long>() { 54,65,75,74 },
            old => old + 6,
            Monkey.CreateTest(19,2,0)
        ),
        new Monkey(
            new List<long>() { 79,60,97 },
            old => old * old,
            Monkey.CreateTest(13,1,3)
        ),
        new Monkey(
            new List<long>() { 74 },
            old => old + 3,
            Monkey.CreateTest(17,0,1)
        ),
    };

    private static List<Monkey> Input => new()
    {
        new Monkey(
            new () { 53, 89, 62, 57, 74, 51, 83, 97 },
            old => old * 3,
            Monkey.CreateTest(13,1,5)
        ),
        new Monkey(
            new List<long>() { 85, 94, 97, 92, 56 },
            old => old + 2,
            Monkey.CreateTest(19,5,2)
        ),
        new Monkey(
            new List<long>() { 86, 82, 82 },
            old => old + 1,
            Monkey.CreateTest(11,3,4)
        ),
        new Monkey(
            new List<long>() { 94, 68},
            old => old + 5,
            Monkey.CreateTest(17,7,6)
        ),
        new Monkey(
            new List<long>() { 83, 62, 74, 58, 96, 68, 85 },
            old => old + 4,
            Monkey.CreateTest(3,3,6)
        ),
        new Monkey(
            new List<long>() { 50, 68, 95, 82 },
            old => old + 8,
            Monkey.CreateTest(7,2,4)
        ),
        new Monkey(
            new List<long>(){ 75 },
            old => old * 7,
            Monkey.CreateTest(5,7,0)
        ),
        new Monkey(
            new List<long>() { 92, 52, 85, 89, 68, 82 },
            old => old * old,
            Monkey.CreateTest(2,0,1)
        ),
    };
}