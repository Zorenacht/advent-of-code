using System.Diagnostics;
using Tools.Geometry;
namespace AoC_2022;


public sealed partial class Day15 : Day
{
    [Test]
    public void Example() => BeaconExclusion.Parse(InputExample).Simulate(10).Should().Be(26);
    [Test]
    public void Part1() => BeaconExclusion.Parse(InputPart1).Simulate(2_000_000).Should().Be(5144286);

    [Test]
    public void ExampleP2() => BeaconExclusion.Parse(InputExample).TuningFrequency(20).Should().Be(56000011);
    [Test]
    public void Part2() => BeaconExclusion.Parse(InputPart1).TuningFrequency(4_000_000).Should().Be(10229191267339L);

    private class BeaconExclusion
    {
        List<Measurement> Measurements { get; set; } = new List<Measurement>();
        HashSet<int> ExclusionsInRow { get; set; } = new HashSet<int>();

        public BeaconExclusion(List<Measurement> measurements) => Measurements = measurements;

        public int Simulate(int row)
        {
            var beacons = Measurements
                .Where(m => m.Beacon.Y == row)
                .Select(m => m.Beacon.X)
                .GroupBy(x => x)
                .Select(m => m.First())
                .ToHashSet();
            foreach (var m in Measurements)
            {
                var rowToSensorYdifference = Math.Abs(row - m.Sensor.Y);
                if (rowToSensorYdifference <= m.Dist)
                {
                    var diff = m.Dist - rowToSensorYdifference;
                    for (int i = m.Sensor.X - diff; i <= m.Sensor.X + diff; i++)
                    {
                        if (!beacons.Contains(i))
                            ExclusionsInRow.Add(i);
                    }
                }
            }
            return ExclusionsInRow.Count();
        }

        private bool Valid(Point p, int max)
        {
            return p.X >= 0 && p.X <= max && p.Y >= 0 && p.Y <= max;
        }

        public long TuningFrequency(int max)
        {
            var set = new HashSet<Point>(100_000_000);
            foreach (var m in Measurements)
            {
                var w = m.Sensor.X - (m.Dist + 1);
                var e = m.Sensor.X + (m.Dist + 1);
                var s = m.Sensor.Y - (m.Dist + 1);
                var n = m.Sensor.Y + (m.Dist + 1);
                int nY = m.Sensor.Y;
                int sY = m.Sensor.Y;
                for (int x = w; x <= m.Sensor.X; x++)
                {
                    var up = new Point(x, nY);
                    var down = new Point(x, sY);
                    if (Valid(up, max)) set.Add(up);
                    if (Valid(down, max)) set.Add(down);
                    nY++;
                    sY--;
                }
                for (int x = m.Sensor.X + 1; x <= e; x++)
                {
                    var up = new Point(x, nY);
                    var down = new Point(x, sY);
                    if (Valid(up, max)) set.Add(up);
                    if (Valid(down, max)) set.Add(down);
                    nY--;
                    sY++;
                }
            }
            foreach (var m in Measurements)
            {
                foreach (var point in set)
                {
                    if (m.Contains(point)) set.Remove(point);
                }
            }
            return set.First().X * 4_000_000L + set.First().Y;
        }

        public record Line(Point from, Point to)
        {
            public Line ExcludeMeasurement(Measurement m)
            {
                /*if (m.LiesInside(from) && m.LiesInside(to)) return null;
                if (!m.LiesInside(from) && !m.LiesInside(to)) return null;*/
                return null;
            }
        }

        public static BeaconExclusion Parse(string[] lines)
        {
            var list = new List<Measurement>();
            foreach (var line in lines)
            {
                var split = line.Split(new string[] { "x=", ", y=", ":" }, StringSplitOptions.None);
                var x1 = int.Parse(split[1]);
                var y1 = int.Parse(split[2]);
                var x2 = int.Parse(split[4]);
                var y2 = int.Parse(split[5]);
                list.Add(new Measurement(new Point(x1, y1), new Point(x2, y2)));
            }
            return new BeaconExclusion(list);
        }

    }

    public record Measurement(Point Sensor, Point Beacon)
    {
        public bool Contains(Point point) => Math.Abs(point.X - Sensor.X) + Math.Abs(point.Y - Sensor.Y) <= Dist;
        public int XDist => Beacon.X - Sensor.X;
        public int YDist => Beacon.Y - Sensor.Y;
        public int Dist => Math.Abs(XDist) + Math.Abs(YDist);
    }
}