using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;
using Tools;

namespace AoC_2023;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class PuzzleAttribute(object? answer) : NUnitAttribute, ITestBuilder, IImplyFixture
{
    private readonly object? _answer = answer;

    public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test? suite)
    {
        var parameters = new TestCaseParameters()
        {
            ExpectedResult = _answer,
        };
        var test = new NUnitTestCaseBuilder()
            .BuildTestMethod(method, suite, parameters);
        test.Name = TestName(method, _answer);
        yield return test;
    }

    private static string TestName(IMethodInfo method, object? input)
        => $"{method.Name[..^1]} {method.Name[^1]}: {(input is { } ? $"Answer is {input}" : "Calculating answer")}";
}
