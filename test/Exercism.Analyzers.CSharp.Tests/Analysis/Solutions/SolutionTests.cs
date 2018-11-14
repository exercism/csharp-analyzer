using System;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Xunit;

namespace Exercism.Analyzers.CSharp.Tests.Analysis.Solutions
{
    public class SolutionTests
    {
        [Theory]
        [InlineData("leap", "Leap")]
        [InlineData("two-fer", "TwoFer")]
        [InlineData("largest-series-product", "LargestSeriesProduct")]
        public void NameIsCorrectlyInferredFromSlug(string slug, string expectedName)
        {
            var exercise = new Solution(slug, Guid.NewGuid().ToString());
            
            Assert.Equal(expectedName, exercise.Name);
        }
    }
}