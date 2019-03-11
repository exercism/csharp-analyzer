namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class LeapSolutions
    {
        public const string MinimumNumberOfChecks = @"
            internal static class Leap
            {
                public static bool IsLeapYear(int year) =>
                    year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
            }";

        public const string UnneededParentheses = @"
            internal static class Leap
            {
                public static bool IsLeapYear(int year) =>
                    (year % 4 == 0) && ((year % 100 != 0) || (year % 400 == 0));
            }";
    
        public const string MethodWithBlockBody = @"
            internal static class Leap
            {
                public static bool IsLeapYear(int year)
                {
                    return year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
                }
            }";
    }
}