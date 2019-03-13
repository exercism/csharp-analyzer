using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class TwoFerAnalyzerTests : AnalyzerTests
    {
        public TwoFerAnalyzerTests() : base("two-fer", "TwoFer")
        {
        }
        
        [Fact]
        public void ApproveAsOptimalWhenUsingDefaultValueWithStringInterpolationInExpressionBody()
        {
            const string code = @"
                using System;
    
                public static class TwoFer
                {
                    public static string Name(string input = ""you"") =>
                        $""One for {input}, one for me."";
                }";

            ShouldBeApprovedAsOptimal(code);
        }

        [Fact]
        public void ApproveWithCommentWhenUsingDefaultValueWithStringInterpolationInBlockBody()
        {
            const string code = @"
                using System;
    
                public static class TwoFer
                {
                    public static string Name(string input = ""you"")
                    {
                        return $""One for {input}, one for me."";
                    }
                }";

            ShouldBeApprovedWithComment(code, "You could write the method an an expression-bodied member");
        }
        
        [Fact]
        public void ApproveWithCommentWhenUsingDefaultValueWithStringConcatenationInExpressionBody()
        {
            const string code = @"
                using System;

                public static class TwoFer
                {
                    public static string Name(string input = ""you"") =>
                        ""One for "" + input + "", one for me."";
                }";

            ShouldBeApprovedWithComment(code, "You can use string interpolation");
        }

        [Fact]
        public void ApproveWithCommentWhenUsingDefaultValueWithStringConcatenationInBlockBody()
        {
            const string code = @"
                using System;

                public static class TwoFer
                {
                    public static string Name(string input = ""you"")
                    {
                        return ""One for "" + input + "", one for me."";
                    }
                }";

            ShouldBeApprovedWithComment(code, "You can use string interpolation");
        }
        
        [Fact]
        public void ApproveWithCommentWhenUsingDefaultValueWithStringFormatInExpressionBody()
        {
            const string code = @"
                using System;

                public static class TwoFer
                {
                    public static string Name(string input = ""you"") =>
                        string.Format(""One for {0}, one for me."", input);
                }";

            ShouldBeApprovedWithComment(code, "You can use string interpolation");
        }

        [Fact]
        public void ApproveWithCommentWhenUsingDefaultValueWithStringFormatInBlockBody()
        {
            const string code = @"
                using System;

                public static class TwoFer
                {
                    public static string Name(string input = ""you"")
                    {
                        return string.Format(""One for {0}, one for me."", input);
                    }
                }";

            ShouldBeApprovedWithComment(code, "You can use string interpolation");
        }
        
        [Fact]
        public void ApproveAsOptimalWhenUsingStringInterpolationWithInlinedNullCoalescingOperatorInExpressionBody()
        {
            const string code = @"
                using System;
    
                public static class TwoFer
                {
                    public static string Name(string input = null) =>
                        $""One for {input ?? ""you""}, one for me."";
                }";

            ShouldBeApprovedAsOptimal(code);
        }

        [Fact]
        public void ApproveWithCommentWhenUsingStringInterpolationWithInlinedNullCoalescingOperatorInBlockBody()
        {
            const string code = @"
                using System;
    
                public static class TwoFer
                {
                    public static string Name(string input = null)
                    {
                        return $""One for {input ?? ""you""}, one for me."";
                    }
                }";

            ShouldBeApprovedWithComment(code, "You could write the method an an expression-bodied member");
        }

        [Fact]
        public void ApproveAsOptimalWhenUsingStringInterpolationWithNullCoalescingOperatorAndVariableForName()
        {
            const string code = @"
                using System;
    
                public static class TwoFer
                {
                    public static string Name(string input = null)
                    {
                        var name = input ?? ""you"";
                        return $""One for {name}, one for me."";
                    }
                }";

            ShouldBeApprovedAsOptimal(code);
        }

        [Fact]
        public void ApproveWithCommentWhenUsingStringConcatenationWithInlinedNullCoalescingOperatorInExpressionBody()
        {
            const string code = @"
                using System;
    
                public static class TwoFer
                {
                    public static string Name(string input = null) =>
                        ""One for "" + (input ?? ""you"") + "", one for me."";
                }";

            ShouldBeApprovedWithComment(code, "You can use string interpolation");
        }

        [Fact]
        public void ApproveWithCommentWhenUsingStringConcatenationWithInlinedNullCoalescingOperatorInBlockBody()
        {
            const string code = @"
                using System;
    
                public static class TwoFer
                {
                    public static string Name(string input = null)
                    {
                        return ""One for "" + (input ?? ""you"") + "", one for me."";
                    }
                }";

            ShouldBeApprovedWithComment(code, "You can use string interpolation");
        }

        [Fact]
        public void ApproveWithCommentWhenUsingStringFormatWithInlinedNullCoalescingOperatorInExpressionBody()
        {
            const string code = @"
                using System;
    
                public static class TwoFer
                {
                    public static string Name(string input = null) =>
                        string.Format(""One for {0}, one for me."", input ?? ""you"");
                }";

            ShouldBeApprovedWithComment(code, "You can use string interpolation");
        }

        [Fact]
        public void ApproveWithCommentWhenUsingStringFormatWithInlinedNullCoalescingOperatorInBlockBody()
        {
            const string code = @"
                using System;
    
                public static class TwoFer
                {
                    public static string Name(string input = null)
                    {
                        return string.Format(""One for {0}, one for me."", input ?? ""you"");
                    }
                }";

            ShouldBeApprovedWithComment(code, "You can use string interpolation");
        }
    }
}