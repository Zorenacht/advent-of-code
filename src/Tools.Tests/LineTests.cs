using Tools.Shapes;

namespace Shapes.Line_specs;

public class Intersection
{
    [Test]
    public void orthogonal_succeeds()
    {
        var line1 = new StraightLine(new Index2D(0, 1), new Index2D(0, -1));
        var line2 = new StraightLine(new Index2D(1, 0), new Index2D(-1, 0));
        var intersection = line1.Intersection(line2);
        intersection.Should().Be(Index2D.O);
    }

    [Test]
    public void orthogonal_too_short_fails()
    {
        var line1 = new StraightLine(new Index2D(0, 0), new Index2D(0, 5));
        var line2 = new StraightLine(new Index2D(2, 4), new Index2D(3, 4));
        var intersection = line1.Intersection(line2);
        intersection.Should().Be(null);
    }
}