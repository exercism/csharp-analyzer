using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class ErrorTests
    {
        [Fact]
        public void DisapproveWithCommentWhenCodeHasSyntaxErrors()
        {
            const string code = @"
                public static class Gigasecond
                {
                    public static DateTime Add
                }";

            var analysisRun = TestSolutionAnalyzer.Run("errors", "Errors", code);

            Assert.True(analysisRun.DisapproveWithComment);
            Assert.Single(analysisRun.Comments, "Has errors");
        }
    }
}