using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class ErrorTests : AnalyzerTests
    {
        public ErrorTests() : base("errors", "Errors")
        {
        }

        [Fact]
        public void DisapproveWithCommentWhenCodeHasSyntaxErrors()
        {
            const string code = @"
                public static class Gigasecond
                {
                    public static DateTime Add
                }";

            ShouldBeDisapprovedWithComment(code, "Has errors");
        }
    }
}