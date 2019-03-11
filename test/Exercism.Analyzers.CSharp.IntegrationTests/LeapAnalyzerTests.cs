using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class LeapAnalyzerTests
    {
        private const string Slug = "leap";
        private const string Name = "Leap";

        [Fact]
        public void ApproveAsOptimalWhenUsingMinimumNumberOfChecks()
        {
            const string code = @"
                public static class Leap
                {
                    public static bool IsLeapYear(int year) =>
                        year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
                }";

            var analysisRun = TestSolutionAnalyzer.Run(Slug, Name, code);

            Assert.True(analysisRun.ApproveAsOptimal);
            Assert.Empty(analysisRun.Comments);
        }

        [Fact]
        public void ApproveAsOptimalWhenUsingUnneededParentheses()
        {
            const string code = @"
                public static class Leap
                {
                    public static bool IsLeapYear(int year) =>
                        (year % 4 == 0) && ((year % 100 != 0) || (year % 400 == 0));
                }";

            var analysisRun = TestSolutionAnalyzer.Run(Slug, Name, code);

            Assert.True(analysisRun.ApproveAsOptimal);
            Assert.Empty(analysisRun.Comments);
        }

        [Fact]
        public void ApproveWithCommentWhenUsingMethodWithBlockBody()
        {
            const string code = @"
                public static class Leap
                {
                    public static bool IsLeapYear(int year)
                    {
                        return year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
                    }
                }";

            var analysisRun = TestSolutionAnalyzer.Run(Slug, Name, code);

            Assert.True(analysisRun.ApproveWithComment);
            Assert.Single(analysisRun.Comments, "You could write the method an an expression-bodied member");
        }

        [Fact]
        public void DisapproveWithCommentWhenUsingTooManyChecks()
        {
            const string code = @"
                public static class Leap
                {
                    public static bool IsLeapYear(int year) =>
                        year % 4 == 0 && year % 100 != 0 || year % 100 == 0 && year % 400 == 0;
                }";

            var analysisRun = TestSolutionAnalyzer.Run(Slug, Name, code);

            Assert.True(analysisRun.DisapproveWithComment);
            Assert.Single(analysisRun.Comments, "Use minimum number of checks");
        }
    }
}