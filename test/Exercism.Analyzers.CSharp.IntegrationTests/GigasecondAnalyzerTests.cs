using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class GigasecondAnalyzerTests : AnalyzerTests
    {
        public GigasecondAnalyzerTests() : base("gigasecond", "Gigasecond")
        {
        }

        [Fact]
        public void ApproveAsOptimalWhenUsingAddSecondsWithScientificNotationUsingExpressionBody()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate) => birthDate.AddSeconds(1e9);
                }";

            ShouldBeApprovedAsOptimal(code);
        }

        [Fact]
        public void ApproveWithCommentWhenUsingAddSecondsWithScientificNotationUsingBlockBody()
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

            ShouldBeApprovedWithComment(code, "You could write the method an an expression-bodied member");
        }

        [Fact]
        public void ApproveWithCommentWhenUsingAddSecondsWithMathPowUsingExpressionBody()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate) => birthDate.AddSeconds(Math.Pow(10, 9));
                }";

            ShouldBeApprovedWithComment(code, "Use 1e9 instead of Math.Pow(10, 9)");
        }

        [Fact]
        public void ApproveWithCommentWhenUsingAddSecondsWithDigitsWithoutSeparatorUsingExpressionBody()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate) => birthDate.AddSeconds(1000000);
                }";

            ShouldBeApprovedWithComment(code,"Use 1e9 or 1_000_000 instead of 1000000");
        }

        [Fact]
        public void DisapproveWithCommentWhenUsingAddUsingExpressionBody()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate) => birthDate.Add(TimeSpan.FromSeconds(1000000000));
                }";

            ShouldBeDisapprovedWithComment(code, "Use AddSeconds");
        }

        [Fact]
        public void DisapproveWithCommentWhenUsingPlusOperatorUsingExpressionBody()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate) => birthDate + TimeSpan.FromSeconds(1000000000);
                }";

            ShouldBeDisapprovedWithComment(code, "Use AddSeconds");
        }
    }
}