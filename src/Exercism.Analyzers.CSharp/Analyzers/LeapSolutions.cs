namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class LeapSolutions
    {
        public const string MinimumNumberOfChecksInExpressionBody = @"
            public static class Leap
            {
                public static bool IsLeapYear(int year) =>
                    year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
            }";

        public const string MinimumNumberOfChecksInBlockBody = @"
            public static class Leap
            {
                public static bool IsLeapYear(int year)
                {
                    return year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
                }
            }";

        public const string MinimumNumberOfChecksWithParenthesesInExpressionBody = @"
            public static class Leap
            {
                public static bool IsLeapYear(int year) =>
                    year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
            }";

        public const string MinimumNumberOfChecksWithParenthesesInBlockBody = @"
            public static class Leap
            {
                public static bool IsLeapYear(int year)
                {
                    return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
                }
            }";
    }
}