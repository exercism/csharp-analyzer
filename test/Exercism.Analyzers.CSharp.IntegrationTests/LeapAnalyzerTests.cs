using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class LeapAnalyzerTests
    {
        private const string Exercise = "leap";

        [Fact]
        public void ApproveWhenUsingMinimumNumberOfChecks()
        {
            const string code = @"
                public static class Leap
                {
                    public static bool IsLeapYear(int year) =>
                        year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
                }";

            var analysisRun = TestSolutionAnalyzer.Run(Exercise, code);
            
            Assert.True(analysisRun.Success);
            Assert.True(analysisRun.Approved);
            Assert.False(analysisRun.ReferToMentor);
            Assert.Empty(analysisRun.Messages);
        }

        [Fact]
        public void ApproveWhenUsingUnneededParentheses()
        {
            const string code = @"
                public static class Leap
                {
                    public static bool IsLeapYear(int year) =>
                        (year % 4 == 0) && ((year % 100 != 0) || (year % 400 == 0));
                }";

            var analysisRun = TestSolutionAnalyzer.Run(Exercise, code);
            
            Assert.True(analysisRun.Success);
            Assert.True(analysisRun.Approved);
            Assert.False(analysisRun.ReferToMentor);
            Assert.Empty(analysisRun.Messages);
        }

        [Fact]
        public void ApproveWithMessageWhenUsingMethodWithBlockBody()
        {
            const string code = @"
                public static class Leap
                {
                    public static bool IsLeapYear(int year)
                    {
                        return year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
                    }
                }";

            var analysisRun = TestSolutionAnalyzer.Run(Exercise, code);
            
            Assert.True(analysisRun.Success);
            Assert.True(analysisRun.Approved);
            Assert.False(analysisRun.ReferToMentor);
            Assert.Single(analysisRun.Messages, "You could write the method an an expression-bodied member");
        }

        [Fact]
        public void ReferToMentorWithMessageWhenUsingTooManyChecks()
        {
            const string code = @"
                public static class Leap
                {
                    public static bool IsLeapYear(int year) =>
                        year % 4 == 0 && year % 100 != 0 || year % 100 == 0 && year % 400 == 0;
                }";

            var analysisRun = TestSolutionAnalyzer.Run(Exercise, code);
            
            Assert.True(analysisRun.Success);
            Assert.False(analysisRun.Approved);
            Assert.True(analysisRun.ReferToMentor);
            Assert.Single(analysisRun.Messages, "Use minimum number of checks");
        }
    }
}