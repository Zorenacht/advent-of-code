using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace AoC;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class PuzzleAttribute(object? answer) : TestAttribute, ITestBuilder
{
    public new IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test? suite)
    {
        var parameters = new TestCaseParameters()
        {
            ExpectedResult = answer,
        };
        var test = new NUnitTestCaseBuilder()
            .BuildTestMethod(method, suite, parameters);
        test.Name = TestName(method, answer);
        yield return test;
    }

    private static string TestName(IMethodInfo method, object? input)
        => $"{method.Name[..4]} {method.Name[4..]}: {(input is null ? "Calculating answer" : $"Answer is {input}")}";
}
