namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class TwoFerSolutions
    {
        public const string DefaultValueWithStringInterpolationInExpressionBody = @"
            using System;

            public static class TwoFer
            {
                public static string Name(string input = ""you"") =>
                    $""One for {input}, one for me."";
            }";

        public const string DefaultValueWithStringInterpolationInBlockBody = @"
            using System;

            public static class TwoFer
            {
                public static string Name(string input = ""you"")
                {
                    return $""One for {input}, one for me."";
                }
            }";

        public const string DefaultValueWithStringConcatenationInExpressionBody = @"
            using System;

            public static class TwoFer
            {
                public static string Name(string input = ""you"") =>
                    ""One for "" + input + "", one for me."";
            }";

        public const string DefaultValueWithStringConcatenationInBlockBody = @"
            using System;

            public static class TwoFer
            {
                public static string Name(string input = ""you"")
                {
                    return ""One for "" + input + "", one for me."";
                }
            }";

        public const string DefaultValueWithStringFormatInExpressionBody = @"
            using System;

            public static class TwoFer
            {
                public static string Name(string input = ""you"") =>
                    string.Format(""One for {0}, one for me."", input);
            }";

        public const string DefaultValueWithStringFormatInBlockBody = @"
            using System;

            public static class TwoFer
            {
                public static string Name(string input = ""you"")
                {
                    return string.Format(""One for {0}, one for me."", input);
                }
            }";

        public const string StringInterpolationWithInlinedNullCoalescingOperatorInExpressionBody = @"
            using System;

            public static class TwoFer
            {
                public static string Name(string input = null) =>
                    $""One for {input ?? ""you""}, one for me."";
            }";

        public const string StringInterpolationWithInlinedNullCoalescingOperatorInBlockBody = @"
            using System;

            public static class TwoFer
            {
                public static string Name(string input = null)
                {
                    return $""One for {input ?? ""you""}, one for me."";
                }
            }";

        public const string StringInterpolationWithTernaryOperatorInExpressionBody = @"
            using System;

            public static class TwoFer
            {
                public static string Name(string input = null) =>
                    $""One for {input == null ? ""you"" : input}, one for me."";
            }";

        public const string StringInterpolationWithTernaryOperatorInBlockBody = @"
            using System;

            public static class TwoFer
            {
                public static string Name(string input = null)
                {
                    return $""One for {input == null ? ""you"" : input}, one for me."";
                }
            }";

        public const string StringInterpolationWithNullCoalescingOperatorAndVariableForName = @"
            using System;

            public static class TwoFer
            {
                public static string Name(string input = null)
                {
                    var name = input ?? ""you"";
                    return $""One for {name}, one for me."";
                }
            }";

        public const string StringConcatenationWithInlinedNullCoalescingOperatorInExpressionBody = @"
            using System;

            public static class TwoFer
            {
                public static string Name(string input = null) =>
                    ""One for "" + (input ?? ""you"") + "", one for me."";
            }";

        public const string StringConcatenationWithInlinedNullCoalescingOperatorInBlockBody = @"
            using System;

            public static class TwoFer
            {
                public static string Name(string input = null)
                {
                    return ""One for "" + (input ?? ""you"") + "", one for me."";
                }
            }";

        public const string StringFormatWithInlinedNullCoalescingOperatorInExpressionBody = @"
            using System;

            public static class TwoFer
            {
                public static string Name(string input = null) =>
                    string.Format(""One for {0}, one for me."", input ?? ""you"");
            }";

        public const string StringFormatWithInlinedNullCoalescingOperatorInBlockBody = @"
            using System;

            public static class TwoFer
            {
                public static string Name(string input = null)
                {
                    return string.Format(""One for {0}, one for me."", input ?? ""you"");
                }
            }";
    }
}