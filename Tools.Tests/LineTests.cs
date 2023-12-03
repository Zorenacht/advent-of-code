using NUnit.Framework.Legacy;
using Tools.Shapes;

namespace Tools.Tests;

public class LineTests
{
    [Test]
    public void OrthogonalIntersection()
    {
        var line1 = new StraightLine(new Point(0, 1), new Point(0, -1));
        var line2 = new StraightLine(new Point(1, 0), new Point(-1, 0));
        var intersection = line1.Intersection(line2);
        intersection.Should().Be(Point.O);
    }

    [Test]
    public void OrthogonalIntersection_Fails()
    {
        var line1 = new StraightLine(new Point(0, 1), new Point(0, -1));
        var line2 = new StraightLine(new Point(1, 2), new Point(-1, 2));
        var intersection = line1.Intersection(line2);
        intersection.Should().Be(null);
    }
}