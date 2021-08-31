using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Xunit.Sdk;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal class TestSolutionsDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod) =>
            TestSolutionsReader.ReadAll().Select(testSolution => new[] { testSolution });
    }
}