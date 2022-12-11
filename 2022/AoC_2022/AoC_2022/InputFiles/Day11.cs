namespace AoC_2022;

public sealed partial class Day11
{
    private static List<Monkey> ExampleInput =>
        new List<Monkey>() {
            new Monkey
            {
                Items = new List<long>() { 79, 98 },
                Operation = (old) => old * 19,
                Throw = Monkey.CreateTest(23,2,3),
            },
            new Monkey
            {
                Items = new List<long>() {54, 65, 75, 74 },
                Operation = (old) => old + 6,
                Throw = Monkey.CreateTest(19,2,0),
            },
            new Monkey
            {
                Items = new List<long>() {79, 60, 97},
                Operation = (old) => old * old,
                Throw = Monkey.CreateTest(13,1,3),
            },
            new Monkey
            {
                Items = new List<long>() { 74},
                Operation = old => old + 3,
                Throw = Monkey.CreateTest(17,0,1),
            },
        };

    private static List<Monkey> Input =>
        new List<Monkey>() {
            new Monkey
            {
                Items = new List<long>() { 53, 89, 62, 57, 74, 51, 83, 97 },
                Operation = (old) => old * 3,
                Throw = Monkey.CreateTest(13,1,5),
            },
            new Monkey
            {
                Items = new List<long>() { 85, 94, 97, 92, 56 },
                Operation = (old) => old + 2,
                Throw = Monkey.CreateTest(19,5,2),
            },
            new Monkey
            {
                Items = new List<long>() { 86, 82, 82},
                Operation = (old) => old + 1,
                Throw = Monkey.CreateTest(11,3,4),
            },
            new Monkey
            {
                Items = new List<long>() { 94, 68},
                Operation = (old) => old + 5,
                Throw = Monkey.CreateTest(17,7,6),
            },
            new Monkey
            {
                Items = new List<long>() { 83, 62, 74, 58, 96, 68, 85 },
                Operation = (old) => old + 4,
                Throw = Monkey.CreateTest(3,3,6),
            },
            new Monkey
            {
                Items = new List<long>() { 50, 68, 95, 82 },
                Operation = (old) => old + 8,
                Throw = Monkey.CreateTest(7,2,4),
            },
            new Monkey
            {
                Items = new List<long>() { 75},
                Operation = (old) => old * 7,
                Throw = Monkey.CreateTest(5,7,0),
            },
            new Monkey
            {
                Items = new List<long>() { 92, 52, 85, 89, 68, 82 },
                Operation = (old) => old * old,
                Throw = Monkey.CreateTest(2,0,1),
            },
        };
}