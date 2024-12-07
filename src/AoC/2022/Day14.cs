using Tools.Geometry;

namespace AoC_2022;


public sealed partial class Day14 : Day
{
    [Test]
    public void Example() => Cave.Parse(InputExample).Simulate().Should().Be(24);
    [Test]
    public void Part1() => Cave.Parse(InputPart1).Simulate().Should().Be(961);

    [Test]
    public void ExampleP2() => Cave.Parse(InputExample).AddFloor().Simulate2().Should().Be(93);
    [Test]
    public void Part2() => Cave.Parse(InputPart1).AddFloor().Simulate2().Should().Be(26375);

    public class Cave
    {
        Grid<int> CaveGrid { get; set; }
        readonly Index2D Sand = new(0, 500);
        int HighestRock => CaveGrid.Lattice.Select(x => x.Contains(1)).ToList().FindLastIndex(val => val == true);

        public int SandCount => CaveGrid.Enumerable().Count(val => val == 2);

        public Cave(Grid<int> cave) => CaveGrid = cave;


        public int Simulate()
        {
            Index2D current = Sand;
            int count = 0;
            while (current.Row < CaveGrid.RowLength - 1)
            {
                if (CaveGrid.ValueAt(current.Neighbor(Direction.S)) == 0) current = current.Neighbor(Direction.S);
                else if (CaveGrid.ValueAt(current.Neighbor(Direction.SW)) == 0) current = current.Neighbor(Direction.SW);
                else if (CaveGrid.ValueAt(current.Neighbor(Direction.SE)) == 0) current = current.Neighbor(Direction.SE);
                else
                {
                    CaveGrid.UpdateAt(current, 2);
                    current = Sand;
                }
                count++;
            }
            return SandCount;
        }

        public int Simulate2()
        {
            Index2D current = Sand;
            int count = 0;
            while (CaveGrid.ValueAt(Sand) == 0)
            {
                if (CaveGrid.ValueAt(current.Neighbor(Direction.S)) == 0) current = current.Neighbor(Direction.S);
                else if (CaveGrid.ValueAt(current.Neighbor(Direction.SW)) == 0) current = current.Neighbor(Direction.SW);
                else if (CaveGrid.ValueAt(current.Neighbor(Direction.SE)) == 0) current = current.Neighbor(Direction.SE);
                else
                {
                    CaveGrid.UpdateAt(current, 2);
                    current = Sand;
                }
                count++;
            }
            return SandCount;
        }

        public static Cave Parse(string[] lines)
        {
            var grid = new Grid<int>(200, 1000);
            foreach (var line in lines)
            {
                //parse input
                var cmds = line.Split(" -> ")
                    .Select(pair =>
                    {
                        var splittedPair = pair.Split(',');
                        return new Index2D(int.Parse(splittedPair[1]), int.Parse(splittedPair[0]));
                    }).ToList();

                //set rocks
                var prevCmd = cmds.First();
                foreach (var cmd in cmds.Skip(1))
                {
                    grid.ApplyRange(prevCmd, cmd, 1);
                    prevCmd = cmd;
                }
            }
            return new Cave(grid);
        }

        public Cave AddFloor()
        {
            CaveGrid.ApplyRange(new Index2D(HighestRock + 2, 0), new Index2D(HighestRock + 2, CaveGrid.ColLength - 1), 1);
            return this;
        }
    }
}