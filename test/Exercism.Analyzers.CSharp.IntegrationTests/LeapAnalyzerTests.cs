using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class LeapAnalyzerTests : AnalyzerTests
    {
        public LeapAnalyzerTests() : base("leap", "Leap")
        {
        }

        [Fact]
        public void ApproveAsOptimalWhenUsingMinimumNumberOfChecksInExpressionBody()
        {
            const string code = @"
                public static class Leap
                {
                    public static bool IsLeapYear(int year) =>
                        year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
                }";

            ShouldBeApprovedAsOptimal(code);
        }

        [Fact]
        public void ApproveWithCommentWhenUsingMinimumNumberOfChecksUsingBlockBody()
        {
            const string code = @"
                public static class Leap
                {
                    public static bool IsLeapYear(int year)
                    {
                        return year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
                    }
                }";

            ShouldBeApprovedWithComment(code, "You could write the method an an expression-bodied member");
        }

        [Fact]
        public void ApproveAsOptimalWhenUsingMinimumNumberOfChecksWithUnneededParenthesesUsingExpressionBody()
        {
            const string code = @"
                public static class Leap
                {
                    public static bool IsLeapYear(int year) =>
                        (year % 4 == 0) && ((year % 100 != 0) || (year % 400 == 0));
                }";

            ShouldBeApprovedAsOptimal(code);
        }

        [Fact]
        public void ApproveWithCommentWhenUsingMinimumNumberOfChecksWithUnneededParenthesesUsingBlockBody()
        {
            const string code = @"
                public static class Leap
                {
                    public static bool IsLeapYear(int year)
                    {
                        return (year % 4 == 0) && ((year % 100 != 0) || (year % 400 == 0));
                    }
                }";

            ShouldBeApprovedWithComment(code, "You could write the method an an expression-bodied member");
        }

        [Fact]
        public void DisapproveWithCommentWhenUsingTooManyChecksInExpressionBody()
        {
            const string code = @"
                public static class Leap
                {
                    public static bool IsLeapYear(int year) =>
                        year % 4 == 0 && year % 100 != 0 || year % 100 == 0 && year % 400 == 0;
                }";

            ShouldBeDisapprovedWithComment(code, "Use minimum number of checks");
        }

        [Fact]
        public void DisapproveWithCommentWhenUsingTooManyChecksUsingBlockBody()
        {
            const string code = @"
                public static class Leap
                {
                    public static bool IsLeapYear(int year)
                    {
                        return year % 4 == 0 && year % 100 != 0 || year % 100 == 0 && year % 400 == 0;
                    }
                }";

            ShouldBeDisapprovedWithComment(code, "Use minimum number of checks");
        }
    }
}