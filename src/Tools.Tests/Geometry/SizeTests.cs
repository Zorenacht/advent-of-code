using Tools.Geometry;

namespace Tools.Tests.Geometry;

internal static class SizeTestCases
{
    public static IEnumerable<int> Int() => [4];
    public static IEnumerable<long> Long() => [8];
}

public class SizeTests
{
    public static IEnumerable<object> Int() => [4];
    public static IEnumerable<long> Long() => [8];

    [Theory]
    [TestCaseGeneric<int>(4)]
    [TestCaseGeneric<long>(8)]
    [TestCaseGeneric<double>(8)]
    [TestCaseGeneric<Index2D>(8)]
    public unsafe int TypeIsOfSize<T>() where T : unmanaged
        => sizeof(T);
}
