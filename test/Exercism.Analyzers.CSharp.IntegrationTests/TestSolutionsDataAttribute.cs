using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class TestSolutionsDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod) =>
            TestSolutionReader.ReadAll().Select(testSolution => new[] { testSolution });
    }
}