using System.Runtime.InteropServices;
using Tools;
using Tools.Geometry;

namespace Collections.Geometry;

public class GridFloodFillTests
{
    private string CharGrid =>
        """
        ......
        .1234.
        .3..4.
        .5..6.
        .789..
        ......
        """;

    private HashSet<Index2D> Outer =>
    [
        new Index2D(0, 0), new Index2D(0, 1), new Index2D(0, 2), new Index2D(0, 3), new Index2D(0, 4), new Index2D(0, 5),
        new Index2D(1, 0), /*                                                                       */ new Index2D(1, 5),
        new Index2D(2, 0), /*                                                                       */ new Index2D(2, 5),
        new Index2D(3, 0), /*                                                                       */ new Index2D(3, 5),
        new Index2D(4, 0), /*                                                    */ new Index2D(4, 4), new Index2D(4, 5),
        new Index2D(5, 0), new Index2D(5, 1), new Index2D(5, 2), new Index2D(5, 3), new Index2D(5, 4), new Index2D(5, 5),
    ];

    private HashSet<Index2D> Inner =>
    [
        new Index2D(2, 2), new Index2D(2, 3),
        new Index2D(3, 2), new Index2D(3, 3),
    ];

    [Test]
    public void FloodFill_DisallowDiagonals()
    {
        var grid = CharGrid.ToGrid();

        var areas = grid.FloodFill("1234567890");

        var outer = areas.KeyedAreas[0];
        var inner = areas.KeyedAreas[1];
        outer.Should().BeEquivalentTo(Outer);
        inner.Should().BeEquivalentTo(Inner);
    }

    [Test]
    public void FloodFill_AllowDiagonals()
    {
        var grid = CharGrid.ToGrid();

        var areas = grid.FloodFill("1234567890", true);

        areas.KeyedAreas.Should().HaveCount(1);
        var area = areas.KeyedAreas[0];
        area.Should().HaveCount(25);
        area.Should().BeEquivalentTo(Outer.Union(Inner));
    }
}