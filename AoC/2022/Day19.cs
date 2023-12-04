using System.Text.RegularExpressions;
using Tools.Geometry;

namespace AoC_2022;


public sealed partial class Day19 : Day
{
    [Test]
    public void Example() => Geodes.Parse(InputExample).Max().Should().Be(33);
    [Test]
    public void Part1() => Geodes.Parse(InputPart1).Max().Should().Be(1650);

    [Test]
    public void ExampleP2() => Geodes.Parse(InputExample).Top3().Should().Be(3472);
    [Test]
    public void Part2() => Geodes.Parse(InputPart1).Top3().Should().Be(5824);

    private class Geodes
    {
        private readonly List<Blueprint> Blueprints;
        private readonly Dictionary<long, int> Memoized;

        public Geodes(List<Blueprint> blueprints)
        {
            Blueprints = blueprints;
            Memoized = new Dictionary<long, int>();
        }

        public int Max()
        {
            int count = 0;
            foreach (var blueprint in Blueprints)
            {
                count += blueprint.No * Recursive(
                    1 + ((long)24 << _timeStart) + ((long)blueprint.No << _blueprintStart),
                    blueprint
                );
            }
            return count;
        }

        public int Top3()
        {
            int prod = 1;
            foreach (var blueprint in Blueprints.Take(3))
            {
                prod *= Recursive(
                    1 + ((long)32 << _timeStart) + ((long)blueprint.No << _blueprintStart),
                    blueprint
                );
                Memoized.Clear();
            }
            return prod;
        }

        const int _robotBits = 5;
        const int _resourceBits = 8;
        const int _timeBits = 6;
        const int _blueprintBits = 5;

        const int _robotStart = 0;
        const int _resourceStart = 20;
        const int _timeStart = 52;
        const int _blueprintStart = 58;

        public static int ResourceStart(int resourceIndex) => _resourceStart + _resourceBits * resourceIndex;
        public static int RobotStart(int resourceIndex) => _robotBits * resourceIndex;

        public static int ResourceAmount(long state, int resourceIndex) => (int)Bits.Extract(state, ResourceStart(resourceIndex), _resourceBits);
        public static int RobotAmount(long state, int resourceIndex) => (int)Bits.Extract(state, RobotStart(resourceIndex), _robotBits);
        public static int Time(long state) => (int)Bits.Extract(state, _timeStart, _timeBits);

        public long SubstractRobotResource(long state, int robotIndex, Blueprint blueprint)
        {
            if (robotIndex == 0)
            {
                state -= (long)blueprint.RobotOre << ResourceStart(0);
            }
            else if (robotIndex == 1)
            {
                state -= (long)blueprint.ClayOre << ResourceStart(0);
            }
            else if (robotIndex == 2)
            {
                state -= (long)blueprint.ObsidianOre << ResourceStart(0);
                state -= (long)blueprint.ObsidianClay << ResourceStart(1);
            }
            else if (robotIndex == 3)
            {
                state -= (long)blueprint.GeodeOre << ResourceStart(0);
                state -= (long)blueprint.GeodeObsidian << ResourceStart(2);
            }
            return state;
        }

        private static long AddTurnResources(long state, int turns)
        {
            for (int i = 0; i < 4; i++)
            {
                state += ((long)RobotAmount(state, i) * turns) << ResourceStart(i);
            }
            return state;
        }


        private int[] TurnsToMakeRobot(long state, Blueprint bp)
        {
            var resources = new int[4] {
                ResourceAmount(state, 0),
                ResourceAmount(state, 1),
                ResourceAmount(state, 2),
                ResourceAmount(state, 3),
            };
            var robots = new int[4]
            {
                RobotAmount(state, 0),
                RobotAmount(state, 1),
                RobotAmount(state, 2),
                RobotAmount(state, 3),
            };
            var a = bp.TurnsToMakeRobot(robots, resources);
            return a;
        }

        public int Recursive(long state, Blueprint blueprint)
        {
            int time = Time(state);

            if (time == 0) return ResourceAmount(state, 3);
            if (Memoized.TryGetValue(state, out var val))
            {
                return val;
            }

            var turns = TurnsToMakeRobot(state, blueprint);
            var list = new List<int>();
            //start with geode robot first
            for (int robotIndex = 3; robotIndex >= 0; robotIndex--)
            {
                if (RobotAmount(state, robotIndex) >= blueprint.MaxNeeded[robotIndex])
                {
                    continue;
                }
                if (turns[robotIndex] <= 0 || time - turns[robotIndex] < 0) continue;
                var newState = AddTurnResources(state, turns[robotIndex]);
                newState += 1L << RobotStart(robotIndex);
                newState = SubstractRobotResource(newState, robotIndex, blueprint);
                newState -= (long)turns[robotIndex] << _timeStart;
                list.Add(Recursive(newState, blueprint));
                if (robotIndex == 3 && turns[3] == 1) break;
            }
            var max = list.Count > 0 ? Math.Max(list.Max(), ResourceAmount(state, 3)) : ResourceAmount(state, 3);
            Memoized.Add(state, max);
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
        public int[] MaxNeeded = new int[4] {
            Math.Max(Math.Max(RobotOre, ClayOre), Math.Max(ObsidianOre, GeodeOre)),
            ObsidianClay,
            GeodeObsidian,
            int.MaxValue,
        };

        public int[] TurnsToMakeRobot(int[] robots, int[] resources)
        {
            var oreRobotTurns = Math.Ceiling(Math.Max(RobotOre - resources[0], 0) / (float)robots[0]);
            var clayRobotTurns = Math.Ceiling(Math.Max(ClayOre - resources[0], 0) / (float)robots[0]);
            var obsidianRobotTurns = Math.Max(
                Math.Ceiling(Math.Max(ObsidianOre - resources[0], 0) / (float)robots[0]),
                Math.Ceiling(Math.Max(ObsidianClay - resources[1], 0) / (float)robots[1]));
            var geodeRobotTurns = Math.Max(
                Math.Ceiling(Math.Max(GeodeOre - resources[0], 0) / (float)robots[0]),
                Math.Ceiling(Math.Max(GeodeObsidian - resources[2], 0) / (float)robots[2]));
            return new int[4] {
                (int)oreRobotTurns + 1,
                (int)clayRobotTurns + 1,
                (int)obsidianRobotTurns + 1,
                (int)geodeRobotTurns + 1,
            };
        }
    }
}
