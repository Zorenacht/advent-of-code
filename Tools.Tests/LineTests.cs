using Tools.Shapes;

namespace Tools.Tests;

public class LineTests
{
    [Test]
    public void OrthogonalIntersection()
    {
        var line1 = new StraightLine(new Shapes.Index2D(0, 1), new Shapes.Index2D(0, -1));
        var line2 = new StraightLine(new Shapes.Index2D(1, 0), new Shapes.Index2D(-1, 0));
        var intersection = line1.Intersection(line2);
        intersection.Should().Be(Shapes.Index2D.O);
    }

    [Test]
    public void OrthogonalIntersection_Fails()
    {
        var line1 = new StraightLine(new Shapes.Index2D(0, 0), new Shapes.Index2D(0, 5));
        var line2 = new StraightLine(new Shapes.Index2D(2, 4), new Shapes.Index2D(3, 4));
        var intersection = line1.Intersection(line2);
        intersection.Should().Be(null);
    }
}