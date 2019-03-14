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

            await ShouldBeApprovedWithComment(code, "csharp.general.use_expression_bodied_member");
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

            await ShouldBeApprovedWithComment(code, "csharp.gigasecond.use_1e9_not_math_pow");
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

            await ShouldBeApprovedWithComment(code, "csharp.gigasecond.use_1e9_not_math_pow");
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

            await ShouldBeApprovedWithComment(code,"csharp.gigasecond.use_1e9_or_digit_separator");
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

            await ShouldBeApprovedWithComment(code,"csharp.gigasecond.use_1e9_or_digit_separator");
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

            await ShouldBeDisapprovedWithComment(code, "csharp.gigasecond.use_add_seconds");
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

            await ShouldBeDisapprovedWithComment(code, "csharp.gigasecond.use_add_seconds");
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

            await ShouldBeDisapprovedWithComment(code, "csharp.gigasecond.use_add_seconds");
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

            await ShouldBeDisapprovedWithComment(code, "csharp.gigasecond.use_add_seconds");
        }
    }
}