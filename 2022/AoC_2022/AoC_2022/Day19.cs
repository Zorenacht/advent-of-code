using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;
using NUnit.Framework.Constraints;
using System.Text;
using System.Text.RegularExpressions;

namespace AoC_2022;


public sealed partial class Day19 : Day
{
    [Test]
    public void Example() => Geodes.Parse(InputExample).Max().Should().Be(33);
    [Test]
    public void Part1() => Geodes.Parse(InputPart1).Max().Should().Be(-100);

    [Test]
    public void ExampleP2() => Geodes.Parse(InputExample).Top3().Should().Be(-100);
    [Test]
    public void Part2() => Geodes.Parse(InputPart1).Top3().Should().Be(-100);

    private class Geodes
    {
        private readonly List<Blueprint> Blueprints;
        private readonly Dictionary<string,int> Memoized;
        private readonly HashSet<int> Outside;

        public Geodes(List<Blueprint> blueprints)
        {
            Blueprints = blueprints;
            Memoized = new Dictionary<string,int>();
            Outside = new HashSet<int>();
        }

        public int Max()
        {
            int count = 0;
            foreach(Blueprint blueprint in Blueprints)
            {
                count += blueprint.No * Recursive(
                    new int[4] { 1, 0, 0, 0 },
                    new int[4] { 0, 0, 0, 0 },
                    24,
                    blueprint);
            }
            return count;
        }
        public int Top3()
        {
            var list = new List<int>();
            foreach (Blueprint blueprint in Blueprints)
            {
                list.Add(blueprint.No * Recursive(
                    new int[4] { 1, 0, 0, 0 },
                    new int[4] { 0, 0, 0, 0 },
                    24,
                    blueprint));
            }
            var top3 = list.Take(3).ToArray();
            return top3[0] * top3[1] * top3[2];
        }

        public string Encode(int[] robots, int[] resources, int time, Blueprint blueprint)
        {
            var sb = new StringBuilder();
            sb.Append(string.Join("", robots.Select(x => x.ToString("000"))));
            sb.Append(string.Join("", resources.Select(x => x.ToString("000"))));
            sb.Append(time.ToString("000"));
            sb.Append(blueprint.No.ToString("000"));
            return sb.ToString();
        }

        public int Recursive(int[] robots, int[] resources, int time, Blueprint blueprint)
        {
            if (time == 0) return resources[3];
            var encoded = Encode(robots,resources,time,blueprint);
            if (Memoized.TryGetValue(encoded, out var val))
            {
                return val;
            }

            var turns = blueprint.TurnsToMakeRobot(robots, resources);
            var list = new List<int>();
            //start with geode robot first
            for (int i = 3; i >= 0; i--)
            {
                if (turns[i] <= 0 || time - turns[i] < 0) continue;
                var newRobots = (int[])robots.Clone();
                newRobots[i] += 1;
                var newResource = robots.Zip(resources, (robot, resource) => turns[i] * robot + resource).ToArray();
                blueprint.SubstractRobotResource(newResource, i);
                list.Add(Recursive(newRobots, newResource, time - turns[i], blueprint));
            }
            var max = list.Count > 0 ? Math.Max(list.Max(), resources[3]) : resources[3];
            Memoized.Add(encoded, max);
            return max;
        }

        public static Geodes Parse(string[] lines)
        {
            var bps = new List<Blueprint>();
            foreach (var line in lines)
            {
                var values = Regex.Matches(line, @"\d+");
                var bp = new Blueprint(
                    int.Parse(values[0].Value),
                    int.Parse(values[1].Value),
                    int.Parse(values[2].Value),
                    int.Parse(values[3].Value),
                    int.Parse(values[4].Value),
                    int.Parse(values[5].Value),
                    int.Parse(values[6].Value));
                bps.Add(bp);
            }
            return new Geodes(bps);
        }
    }

    private record Blueprint(
        int No,
        int RobotOre,
        int ClayOre,
        int ObsidianOre,
        int ObsidianClay,
        int GeodeOre,
        int GeodeObsidian)
    {
        public void SubstractRobotResource(int[] resources, int robot)
        {
            if(robot == 0)
            {
                resources[0] -= RobotOre;
            }
            else if (robot == 1) 
            {
                resources[0] -= ClayOre;
            }
            else if (robot == 2)
            {
                resources[0] -= ObsidianOre;
                resources[1] -= ObsidianClay;
            }
            else if (robot == 3)
            {
                resources[0] -= GeodeOre;
                resources[2] -= GeodeObsidian;
            }
        }

        public int[] TurnsToMakeRobot(int[] robots, int[] resources)
        {
            var oreRobotTurns = Math.Ceiling(Math.Max(RobotOre - resources[0],0) / (float)robots[0]);
            var clayRobotTurns = Math.Ceiling(Math.Max(ClayOre - resources[0],0) / (float)robots[0]);
            var obsidianRobotTurns = Math.Max(
                Math.Ceiling(Math.Max(ObsidianOre - resources[0],0) / (float)robots[0]),
                Math.Ceiling(Math.Max(ObsidianClay - resources[1],0) / (float)robots[1]));
            var geodeRobotTurns = Math.Max(
                Math.Ceiling(Math.Max(GeodeOre - resources[0],0) / (float)robots[0]),
                Math.Ceiling(Math.Max(GeodeObsidian - resources[2],0) / (float)robots[2]));
            return new int[4] {
                (int)oreRobotTurns + 1,
                (int)clayRobotTurns + 1,
                (int)obsidianRobotTurns + 1,
                (int)geodeRobotTurns + 1,
            };
        }
    }
}
