using System.Threading.Tasks;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class LeapAnalyzerTests : AnalyzerTests
    {
        public LeapAnalyzerTests() : base("leap", "Leap")
        {
        }

        [Fact]
        public async Task ApproveAsOptimalWhenUsingMinimumNumberOfChecksInExpressionBody()
        {
            const string code = @"
                public static class Leap
                {
                    public static bool IsLeapYear(int year) =>
                        year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
                }";

            await ShouldBeApprovedAsOptimal(code);
        }

        [Fact]
        public async Task ApproveWithCommentWhenUsingMinimumNumberOfChecksUsingBlockBody()
        {
            const string code = @"
                public static class Leap
                {
                    public static bool IsLeapYear(int year)
                    {
                        return year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
                    }
                }";

            await ShouldBeApprovedWithComment(code, "You could write the method an an expression-bodied member");
        }

        [Fact]
        public async Task ApproveAsOptimalWhenUsingMinimumNumberOfChecksWithParenthesesUsingExpressionBody()
        {
            const string code = @"
                public static class Leap
                {
                    public static bool IsLeapYear(int year) =>
                        year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
                }";

            await ShouldBeApprovedAsOptimal(code);
        }

        [Fact]
        public async Task ApproveWithCommentWhenUsingMinimumNumberOfChecksWithParenthesesUsingBlockBody()
        {
            const string code = @"
                public static class Leap
                {
                    public static bool IsLeapYear(int year)
                    {
                        return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
                    }
                }";

            await ShouldBeApprovedWithComment(code, "You could write the method an an expression-bodied member");
        }

        [Fact]
        public async Task ApproveAsOptimalWhenUsingMinimumNumberOfChecksWithUnneededParenthesesUsingExpressionBody()
        {
            const string code = @"
                public static class Leap
                {
                    public static bool IsLeapYear(int year) =>
                        (year % 4 == 0) && ((year % 100 != 0) || (year % 400 == 0));
                }";

            await ShouldBeApprovedAsOptimal(code);
        }

        [Fact]
        public async Task ApproveWithCommentWhenUsingMinimumNumberOfChecksWithUnneededParenthesesUsingBlockBody()
        {
            const string code = @"
                public static class Leap
                {
                    public static bool IsLeapYear(int year)
                    {
                        return (year % 4 == 0) && ((year % 100 != 0) || (year % 400 == 0));
                    }
                }";

            await ShouldBeApprovedWithComment(code, "You could write the method an an expression-bodied member");
        }

        [Fact]
        public async Task DisapproveWithCommentWhenUsingTooManyChecksInExpressionBody()
        {
            const string code = @"
                public static class Leap
                {
                    public static bool IsLeapYear(int year) =>
                        year % 4 == 0 && year % 100 != 0 || year % 100 == 0 && year % 400 == 0;
                }";

            await ShouldBeDisapprovedWithComment(code, "Use minimum number of checks");
        }

        [Fact]
        public async Task DisapproveWithCommentWhenUsingTooManyChecksUsingBlockBody()
        {
            const string code = @"
                public static class Leap
                {
                    public static bool IsLeapYear(int year)
                    {
                        return year % 4 == 0 && year % 100 != 0 || year % 100 == 0 && year % 400 == 0;
                    }
                }";

            await ShouldBeDisapprovedWithComment(code, "Use minimum number of checks");
        }
    }
}