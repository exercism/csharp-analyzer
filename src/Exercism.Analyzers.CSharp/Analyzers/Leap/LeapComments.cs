namespace Exercism.Analyzers.CSharp.Analyzers.Leap;

internal static class LeapComments
{
    public static readonly SolutionComment DoNotUseIsLeapYear = new("csharp.leap.do_not_use_is_leap_year", SolutionCommentType.Essential);
    public static readonly SolutionComment DoNotUseIfStatement = new("csharp.leap.do_not_use_if_statement", SolutionCommentType.Actionable);
    public static readonly SolutionComment UseMinimumNumberOfChecks = new("csharp.leap.use_minimum_number_of_checks", SolutionCommentType.Actionable);
}