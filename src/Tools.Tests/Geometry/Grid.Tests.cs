using Tools;
using Tools.Collections;
using Tools.Geometry;

namespace Collections.Geometry;

public class GridTests
{
    private string DigitGrid =
        """
        123456789
        987654321
        """;

    [Test]
    public void CreateGrid()
    {
        var grid = new Grid<int>(DigitGrid.Lines().Select(x => x.Select(x => x - '0').ToArray()).ToArray());
        grid.RowLength.Should().Be(2);
        grid.ColLength.Should().Be(9);
    }

    [TestCase(0, 0, ExpectedResult = true)]
    [TestCase(0, 8, ExpectedResult = true)]
    [TestCase(1, 0, ExpectedResult = true)]
    [TestCase(1, 8, ExpectedResult = true)]
    [TestCase(-1, 0, ExpectedResult = false)]
    [TestCase(+0, 9, ExpectedResult = false)]
    [TestCase(+1, 9, ExpectedResult = false)]
    [TestCase(+2, 8, ExpectedResult = false)]
    public bool IsValid(int i, int j)
    {
        var grid = new Grid<int>(DigitGrid.Lines().Select(x => x.Select(x => x - '0').ToArray()).ToArray());
        return grid.IsValid(i, j);
    }

    [TestCase(0, 0, ExpectedResult = 1)]
    [TestCase(0, 8, ExpectedResult = 9)]
    [TestCase(1, 0, ExpectedResult = 9)]
    [TestCase(1, 8, ExpectedResult = 1)]
    [TestCase(-1, 0, ExpectedResult = null)]
    [TestCase(+0, 9, ExpectedResult = null)]
    [TestCase(+1, 9, ExpectedResult = null)]
    [TestCase(+2, 8, ExpectedResult = null)]
    public int? ValueOrDefault(int i, int j)
    {
        var grid = new Grid<int>(DigitGrid.Lines().Select(x => x.Select(x => x - '0').ToArray()).ToArray());
        return grid.ValueOrDefault(i, j);
    }
}