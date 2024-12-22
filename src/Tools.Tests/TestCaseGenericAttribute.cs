using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace Tools.Tests;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class TestCaseGenericAttribute<T>(object? size) : TestAttribute, ITestBuilder
{
    public new IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test? suite)
    {
        if (!method.IsGenericMethodDefinition)
            throw new InvalidOperationException("Attribute can only be used on generic method definitions.");

        // Changing arguments requires tests to be rediscovered
        var parameters = new TestCaseParameters()
        {
            TypeArgs = [typeof(T)],
            ExpectedResult = size,
        };

        var test = new NUnitTestCaseBuilder()
            .BuildTestMethod(method, suite, parameters);

        test.Name = $"{method.Name}<{typeof(T).Name}>: {size}";

        yield return test;
    }
}