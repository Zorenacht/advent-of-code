using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System;


namespace AoC_2022.TestAttributes;

public class Example : TestAttribute
{
    /*public string? FileName { get; set; }

    public void ApplyToTest(Test test)
    {
        Guard.ArgumentValid(test.Method is object, "This attribute must only be applied to tests that have an associated method.", nameof(test));

        if (!test.Properties.ContainsKey(PropertyNames.Description) && Description != null)
            test.Properties.Set(PropertyNames.Description, Description);

        if (!test.Properties.ContainsKey(PropertyNames.Author) && Author != null)
            test.Properties.Set(PropertyNames.Author, Author);

        if (!test.Properties.ContainsKey(PropertyNames.TestOf) && TestOf != null)
            test.Properties.Set(PropertyNames.TestOf, TestOf.FullName);

        if (_hasExpectedResult && test.Method.GetParameters().Length > 0)
            test.MakeInvalid("The 'TestAttribute.ExpectedResult' property may not be used on parameterized methods.");

    }

    /// <summary>
    /// Builds a single test from the specified method and context.
    /// </summary>
    /// <param name="method">The method for which a test is to be constructed.</param>
    /// <param name="suite">The suite to which the test will be added.</param>
    public TestMethod BuildFrom(IMethodInfo method, Test? suite)
    {
        TestCaseParameters? parms = null;

        if (_hasExpectedResult)
        {
            parms = new TestCaseParameters();
            parms.ExpectedResult = ExpectedResult;
        }

        return _builder.BuildTestMethod(method, suite, parms);
    }*/
}