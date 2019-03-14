using System.Threading.Tasks;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class TwoFerAnalyzerTests : AnalyzerTests
    {
        public TwoFerAnalyzerTests() : base("two-fer", "TwoFer")
        {
        }
        
        [Fact]
        public async Task ApproveAsOptimalWhenUsingDefaultValueWithStringInterpolationInExpressionBody()
        {
            const string code = @"
                using System;
    
                public static class TwoFer
                {
                    public static string Name(string input = ""you"") =>
                        $""One for {input}, one for me."";
                }";

            await ShouldBeApprovedAsOptimal(code);
        }

        [Fact]
        public async Task ApproveWithCommentWhenUsingDefaultValueWithStringInterpolationInBlockBody()
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

            await ShouldBeApprovedWithComment(code, "csharp.general.use_expression_bodied_member");
        }
        
        [Fact]
        public async Task ApproveWithCommentWhenUsingDefaultValueWithStringConcatenationInExpressionBody()
        {
            const string code = @"
                using System;

                public static class TwoFer
                {
                    public static string Name(string input = ""you"") =>
                        ""One for "" + input + "", one for me."";
                }";

            await ShouldBeApprovedWithComment(code, "csharp.general.use_string_interpolation_not_string_concatenation");
        }

        [Fact]
        public async Task ApproveWithCommentWhenUsingDefaultValueWithStringConcatenationInBlockBody()
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

            await ShouldBeApprovedWithComment(code, "csharp.general.use_string_interpolation_not_string_concatenation");
        }
        
        [Fact]
        public async Task ApproveWithCommentWhenUsingDefaultValueWithStringFormatInExpressionBody()
        {
            const string code = @"
                using System;

                public static class TwoFer
                {
                    public static string Name(string input = ""you"") =>
                        string.Format(""One for {0}, one for me."", input);
                }";

            await ShouldBeApprovedWithComment(code, "csharp.general.use_string_interpolation_not_string_format");
        }

        [Fact]
        public async Task ApproveWithCommentWhenUsingDefaultValueWithStringFormatInBlockBody()
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

            await ShouldBeApprovedWithComment(code, "csharp.general.use_string_interpolation_not_string_format");
        }
        
        [Fact]
        public async Task ApproveAsOptimalWhenUsingStringInterpolationWithInlinedNullCoalescingOperatorInExpressionBody()
        {
            const string code = @"
                using System;
    
                public static class TwoFer
                {
                    public static string Name(string input = null) =>
                        $""One for {input ?? ""you""}, one for me."";
                }";

            await ShouldBeApprovedAsOptimal(code);
        }

        [Fact]
        public async Task ApproveWithCommentWhenUsingStringInterpolationWithInlinedNullCoalescingOperatorInBlockBody()
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

            await ShouldBeApprovedWithComment(code, "csharp.general.use_expression_bodied_member");
        }

        [Fact]
        public async Task ApproveAsOptimalWhenUsingStringInterpolationWithNullCoalescingOperatorAndVariableForName()
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

            await ShouldBeApprovedAsOptimal(code);
        }

        [Fact]
        public async Task ApproveWithCommentWhenUsingStringConcatenationWithInlinedNullCoalescingOperatorInExpressionBody()
        {
            const string code = @"
                using System;
    
                public static class TwoFer
                {
                    public static string Name(string input = null) =>
                        ""One for "" + (input ?? ""you"") + "", one for me."";
                }";

            await ShouldBeApprovedWithComment(code, "csharp.general.use_string_interpolation_not_string_concatenation");
        }

        [Fact]
        public async Task ApproveWithCommentWhenUsingStringConcatenationWithInlinedNullCoalescingOperatorInBlockBody()
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

            await ShouldBeApprovedWithComment(code, "csharp.general.use_string_interpolation_not_string_concatenation");
        }

        [Fact]
        public async Task ApproveWithCommentWhenUsingStringFormatWithInlinedNullCoalescingOperatorInExpressionBody()
        {
            const string code = @"
                using System;
    
                public static class TwoFer
                {
                    public static string Name(string input = null) =>
                        string.Format(""One for {0}, one for me."", input ?? ""you"");
                }";

            await ShouldBeApprovedWithComment(code, "csharp.general.use_string_interpolation_not_string_format");
        }

        [Fact]
        public async Task ApproveWithCommentWhenUsingStringFormatWithInlinedNullCoalescingOperatorInBlockBody()
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

            await ShouldBeApprovedWithComment(code, "csharp.general.use_string_interpolation_not_string_format");
        }
    }
}