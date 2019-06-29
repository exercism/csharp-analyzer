namespace Exercism.Analyzers.CSharp.Analyzers.Leap
{
    internal static class LeapComments
    {
        public static readonly SolutionComment DoNotUseIsLeapYear = new SolutionComment("csharp.leap.do_not_use_is_leap_year");
        public static readonly SolutionComment DoNotUseIfStatement = new SolutionComment("csharp.leap.do_not_use_if_statement");
        public static readonly SolutionComment UseMinimumNumberOfChecks = new SolutionComment("csharp.leap.use_minimum_number_of_checks");
    }
}