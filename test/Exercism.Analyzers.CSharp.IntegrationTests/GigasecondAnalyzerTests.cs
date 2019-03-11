using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class GigasecondAnalyzerTests
    {
        private const string Slug = "gigasecond";
        private const string Name = "Gigasecond";

        [Fact]
        public void ApproveAsOptimalWhenUsingAddSecondsWithScientificNotation()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate) => birthDate.AddSeconds(1e9);
                }";

            var analysisRun = TestSolutionAnalyzer.Run(Slug, Name, code);

            Assert.True(analysisRun.ApproveAsOptimal);
            Assert.Empty(analysisRun.Comments);
        }

        [Fact]
        public void ApproveWithCommentWhenUsingAddSecondsWithMathPow()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate) => birthDate.AddSeconds(Math.Pow(10, 9));
                }";

            var analysisRun = TestSolutionAnalyzer.Run(Slug, Name, code);

            Assert.True(analysisRun.ApproveWithComment);
            Assert.Single(analysisRun.Comments, "Use 1e9 instead of Math.Pow(10, 9)");
        }

        [Fact]
        public void ApproveWithCommentWhenUsingAddSecondsWithDigitsWithoutSeparator()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate) => birthDate.AddSeconds(1000000);
                }";

            var analysisRun = TestSolutionAnalyzer.Run(Slug, Name, code);

            Assert.True(analysisRun.ApproveWithComment);
            Assert.Single(analysisRun.Comments, "Use 1e9 or 1_000_000 instead of 1000000");
        }

        [Fact]
        public void ApproveWithCommentWhenUsingAddSecondsWithScientificNotationInBlockBody()
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

            var analysisRun = TestSolutionAnalyzer.Run(Slug, Name, code);

            Assert.True(analysisRun.ApproveWithComment);
            Assert.Single(analysisRun.Comments, "You could write the method an an expression-bodied member");
        }

        [Fact]
        public void DisapproveWithCommentWhenUsingAdd()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate) => birthDate.Add(TimeSpan.FromSeconds(1000000000));
                }";

            var analysisRun = TestSolutionAnalyzer.Run(Slug, Name, code);

            Assert.True(analysisRun.DisapproveWithComment);
            Assert.Single(analysisRun.Comments, "Use AddSeconds");
        }

        [Fact]
        public void DisapproveWithCommentWhenUsingPlusOperator()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate) => birthDate + TimeSpan.FromSeconds(1000000000);
                }";

            var analysisRun = TestSolutionAnalyzer.Run(Slug, Name, code);

            Assert.True(analysisRun.DisapproveWithComment);
            Assert.Single(analysisRun.Comments, "Use AddSeconds");
        }
    }
}