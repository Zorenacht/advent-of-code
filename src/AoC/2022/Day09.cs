using Tools.Geometry;

namespace AoC._2022;

public sealed partial class Day09 : Day
{
    [Test]
    public void Example() => Simulate(InputExample, 2).Should().Be(13);

    [Test]
    public void Part1() => Simulate(InputPart1, 2).Should().Be(6044);

    [Test]
    public void Part2() => Simulate(InputPart1, 10).Should().Be(2384);


    private static int Simulate(string[] input, int size)
    {
        var rope = Enumerable.Repeat(Point.O, size).ToArray();
        var visited = new HashSet<Point>();
        foreach (string line in input)
        {
            var dir = StringToDirection(line[0]);
            var amount = int.Parse(line[2..]);
            for (int i = 0; i < amount; i++)
            {
                rope[0] = rope[0].Neighbor(dir);
                UpdateTail(rope);
                visited.Add(rope[^1]);
            }
        }
        return visited.Count;
    }

    private static Direction StringToDirection(char s) => s switch
    {
        'R' => Direction.E,
        'L' => Direction.W,
        'U' => Direction.N,
        'D' => Direction.S,
        _ => throw new IndexOutOfRangeException(),
    };

    private static void UpdateTail(Point[] rope)
    {
        for (int i = 1; i < rope.Length; i++)
        {
            var diff = rope[i - 1].Difference(rope[i]);
            if (diff.Norm > 2)
            {
                rope[i] = rope[i].Neighbor(diff.GeneralDirection);
            }
        }
    }
}