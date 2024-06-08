namespace AoC_2023;

public sealed class Day22 : Day
{
    [Puzzle(answer: 5)]
    public long Part1Example() => new Airship().Part1(InputExample, true);

    [Puzzle(answer: 485)]
    public long Part1() => new Airship().Part1(Input, true);

    [Puzzle(answer: 7)]
    public long Part2Example() => new Airship().Part1(InputExample, false);

    [Puzzle(answer: 74594)]
    public long Part2() => new Airship().Part1(Input, false);


    private class Airship
    {
        internal long Part1(string[] input, bool p1)
        {
            var parsed = input.Select(x => x.Split(",~".ToCharArray()))
                .Select((x, index) => (
                    index,
                    int.Parse(x[0]),
                    int.Parse(x[3]),
                    int.Parse(x[1]),
                    int.Parse(x[4]),
                    int.Parse(x[2]),
                    int.Parse(x[5])
                ));

            var grid = new int[10][][];
            for (int i = 0; i < 10; i++)
            {
                grid[i] = new int[10][];
                for (int j = 0; j < 10; j++)
                {
                    grid[i][j] = Enumerable.Repeat(-1, 500).ToArray();
                }
            }

            //fill grid
            foreach (var brick in parsed)
            {
                for (int i = brick.Item2; i <= brick.Item3; i++)
                {
                    for (int j = brick.Item4; j <= brick.Item5; j++)
                    {
                        for (int z = brick.Item6; z <= brick.Item7; z++)
                        {
                            grid[i][j][z] = brick.Item1;
                        }
                    }
                }
            }

            //move bricks down
            var sortedBricks2 = parsed.OrderBy(x => x.Item6).ToList();
            var temp = sortedBricks2.ToList();
            sortedBricks2.Clear();
            while (temp.Any())
            {
                var moving = temp.ToList();
                moving.Clear();
                foreach (var brick in temp)
                {
                    var supported = false;
                    for (int i = brick.Item2; i <= brick.Item3; i++)
                    {
                        for (int j = brick.Item4; j <= brick.Item5; j++)
                        {
                            if (brick.Item6 == 1) supported = true;
                            if (brick.Item6 > 1 && grid[i][j][brick.Item6 - 1] >= 0)
                            {
                                supported = true;
                            }
                        }
                    }
                    if (!supported)
                    {
                        for (int i = brick.Item2; i <= brick.Item3; i++)
                        {
                            for (int j = brick.Item4; j <= brick.Item5; j++)
                            {
                                for (int z = brick.Item6; z <= brick.Item7; z++)
                                {
                                    grid[i][j][z - 1] = grid[i][j][z];
                                    grid[i][j][z] = -1;
                                }
                            }
                        }
                        moving.Add(new(brick.Item1, brick.Item2, brick.Item3, brick.Item4, brick.Item5, brick.Item6 - 1, brick.Item7 - 1));
                    }
                    else
                    {
                        sortedBricks2.Add(brick);
                    }
                }
                temp = moving;
            }

            // determine the amount of supports one block has
            var supportedBy = new Dictionary<int, HashSet<int>>();
            sortedBricks2 = sortedBricks2.OrderBy(x => x.Item6).ToList();
            foreach (var brick in sortedBricks2)
            {
                var supported = false;
                for (int i = brick.Item2; i <= brick.Item3; i++)
                {
                    for (int j = brick.Item4; j <= brick.Item5; j++)
                    {
                        if (brick.Item6 == 1) supportedBy[brick.Item1] = new HashSet<int>();
                        else if (brick.Item6 > 1 && grid[i][j][brick.Item6 - 1] >= 0)
                        {
                            if (!supportedBy.ContainsKey(brick.Item1)) supportedBy[brick.Item1] = new HashSet<int>();
                            supportedBy[brick.Item1].Add(grid[i][j][brick.Item6 - 1]);
                        }
                    }
                }
            }


            // classify those with 1 support and more
            var needed = new HashSet<int>();
            foreach (var supported in supportedBy)
            {
                if (supported.Value.Count == 1)
                {
                    needed.UnionWith(supported.Value);
                }
            }

            // part 2 determine sum of all falling blocks
            int sum = 0;
            foreach (var neededBlock in needed)
            {
                var allGone = new HashSet<int> { neededBlock };
                while (true)
                {
                    var temp2 = supportedBy
                        .Where(x => x.Value.Count > 0 && allGone.IsSupersetOf(x.Value))
                        .Select(x => x.Key)
                        .ToHashSet();
                    temp2.Add(neededBlock);
                    if (allGone.Count == temp2.Count) break;
                    allGone.UnionWith(temp2);
                }
                sum += allGone.Count - 1;
            }
            return p1 ? sortedBricks2.Count - needed.Count : sum;
        }

    }
}