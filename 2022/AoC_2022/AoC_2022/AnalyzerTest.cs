namespace AoC_2022;

public sealed partial class AnalyzerTest 
{
    [Test]
    public void Example()
    {
        int actual = 1;
        actual.Should().BeGreaterThan(1);
        actual.Should().BePositive();

        int? a = 1;
        a.Should().Be(1);

        TestClass? cl = new TestClass();
        cl?.Value.Should().Be(1);
        cl?.List.FirstOrDefault(x => x.ToString() == null).Should().BeNull();

        (cl?.List).FirstOrDefault(x => x?.ToString() == null).Should().BeNull();
        (cl?.List.FirstOrDefault(x => x.ToString() == null)).Should().BeNull();
        (cl?.List.FirstOrDefault(x => x?.ToString() == null)).Should().BeNull();
    }

    private class TestClass
    {
        public int Value { get; set; }
        public string[] List { get; set; } = Array.Empty<string>();
    }
}
