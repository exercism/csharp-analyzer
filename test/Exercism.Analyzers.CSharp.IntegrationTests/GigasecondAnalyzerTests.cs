using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class GigasecondAnalyzerTests
    {
        private const string Exercise = "gigasecond";
        private const string Name = "Gigasecond";

        [Fact]
        public void ApproveWhenUsingAddSecondsWithScientificNotation()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate) => birthDate.AddSeconds(1e9);
                }";

            var analysisRun = TestSolutionAnalyzer.Run(Exercise, Name, code);

            Assert.True(analysisRun.ApproveAsOptimal);
            Assert.Empty(analysisRun.Comments);
        }

        [Fact]
        public void ApproveWithMessageWhenUsingAddSecondsWithMathPow()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate) => birthDate.AddSeconds(Math.Pow(10, 9));
                }";

            var analysisRun = TestSolutionAnalyzer.Run(Exercise, Name, code);

            Assert.True(analysisRun.ApproveWithComment);
            Assert.Single(analysisRun.Comments, "Use 1e9 instead of Math.Pow(10, 9)");
        }

        [Fact]
        public void ApproveWithMessageWhenUsingAddSecondsWithDigitsWithoutSeparator()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate) => birthDate.AddSeconds(1000000);
                }";

            var analysisRun = TestSolutionAnalyzer.Run(Exercise, Name, code);

            Assert.True(analysisRun.ApproveWithComment);
            Assert.Single(analysisRun.Comments, "Use 1e9 or 1_000_000 instead of 1000000");
        }

        [Fact]
        public void ApproveWithMessageWhenUsingAddSecondsWithScientificNotationInBlockBody()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate)
                    {
                        return birthDate.AddSeconds(1e9);
                    }
                }";

            var analysisRun = TestSolutionAnalyzer.Run(Exercise, Name, code);

            Assert.True(analysisRun.ApproveWithComment);
            Assert.Single(analysisRun.Comments, "You could write the method an an expression-bodied member");
        }

        [Fact]
        public void ReferToMentorWithMessageWhenUsingAdd()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate) => birthDate.Add(TimeSpan.FromSeconds(1000000000));
                }";

            var analysisRun = TestSolutionAnalyzer.Run(Exercise, Name, code);

            Assert.True(analysisRun.DisapproveWithComment);
            Assert.Single(analysisRun.Comments, "Use AddSeconds");
        }

        [Fact]
        public void ReferToMentorWithMessageWhenUsingPlusOperator()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate) => birthDate + TimeSpan.FromSeconds(1000000000);
                }";

            var analysisRun = TestSolutionAnalyzer.Run(Exercise, Name, code);

            Assert.True(analysisRun.DisapproveWithComment);
            Assert.Single(analysisRun.Comments, "Use AddSeconds");
        }
    }
}