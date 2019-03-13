using System.Threading.Tasks;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class GigasecondAnalyzerTests : AnalyzerTests
    {
        public GigasecondAnalyzerTests() : base("gigasecond", "Gigasecond")
        {
        }

        [Fact]
        public async Task ApproveAsOptimalWhenUsingAddSecondsWithScientificNotationInExpressionBody()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate) => birthDate.AddSeconds(1e9);
                }";

            await ShouldBeApprovedAsOptimal(code);
        }

        [Fact]
        public async Task ApproveWithCommentWhenUsingAddSecondsWithScientificNotationUsingBlockBody()
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

            await ShouldBeApprovedWithComment(code, "You could write the method an an expression-bodied member");
        }

        [Fact]
        public async Task ApproveWithCommentWhenUsingAddSecondsWithMathPowInExpressionBody()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate) => birthDate.AddSeconds(Math.Pow(10, 9));
                }";

            await ShouldBeApprovedWithComment(code, "Use 1e9 instead of Math.Pow(10, 9)");
        }

        [Fact]
        public async Task ApproveWithCommentWhenUsingAddSecondsWithMathPowUsingBlockBody()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate) 
                    {
                        return birthDate.AddSeconds(Math.Pow(10, 9));
                    }
                }";

            await ShouldBeApprovedWithComment(code, "Use 1e9 instead of Math.Pow(10, 9)");
        }

        [Fact]
        public async Task ApproveWithCommentWhenUsingAddSecondsWithDigitsWithoutSeparatorInExpressionBody()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate) => birthDate.AddSeconds(1000000);
                }";

            await ShouldBeApprovedWithComment(code,"Use 1e9 or 1_000_000 instead of 1000000");
        }

        [Fact]
        public async Task ApproveWithCommentWhenUsingAddSecondsWithDigitsWithoutSeparatorUsingBlockBody()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate)
                    {
                        return birthDate.AddSeconds(1000000);
                    }
                }";

            await ShouldBeApprovedWithComment(code,"Use 1e9 or 1_000_000 instead of 1000000");
        }

        [Fact]
        public async Task DisapproveWithCommentWhenUsingAddInExpressionBody()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate) => birthDate.Add(TimeSpan.FromSeconds(1000000000));
                }";

            await ShouldBeDisapprovedWithComment(code, "Use AddSeconds");
        }

        [Fact]
        public async Task DisapproveWithCommentWhenUsingAddUsingBlockBody()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate)
                    {
                        return birthDate.Add(TimeSpan.FromSeconds(1000000000));
                    }
                }";

            await ShouldBeDisapprovedWithComment(code, "Use AddSeconds");
        }

        [Fact]
        public async Task DisapproveWithCommentWhenUsingPlusOperatorInExpressionBody()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate) => birthDate + TimeSpan.FromSeconds(1000000000);
                }";

            await ShouldBeDisapprovedWithComment(code, "Use AddSeconds");
        }

        [Fact]
        public async Task DisapproveWithCommentWhenUsingPlusOperatorUsingBlockBody()
        {
            const string code = @"
                using System;
                
                public static class Gigasecond
                {
                    public static DateTime Add(DateTime birthDate)
                    {
                        return birthDate + TimeSpan.FromSeconds(1000000000);
                    }
                }";

            await ShouldBeDisapprovedWithComment(code, "Use AddSeconds");
        }
    }
}