namespace Exercism.Analyzers.CSharp.Analyzers
{
    public static class LeapSolutions
    {
        public const string MinimumNumberOfChecks = @"
            public static class Leap
            {
                public static bool IsLeapYear(int year) =>
                    year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
            }";

        public const string UnneededParentheses = @"
            public static class Leap
            {
                public static bool IsLeapYear(int year) =>
                    (year % 4 == 0) && ((year % 100 != 0) || (year % 400 == 0));
            }";
    
        public const string MethodWithBlockBody = @"
            public static class Leap
            {
                public static bool IsLeapYear(int year)
                {
                    return year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
                }
            }";
    }
}