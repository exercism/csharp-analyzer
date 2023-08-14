using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedCommentParameters;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond;

internal static class GigasecondComments
{
    public static readonly SolutionComment UseAddSeconds = new("csharp.gigasecond.use_add_seconds", SolutionCommentType.Actionable);
    public static readonly SolutionComment DoNotCreateDateTime = new("csharp.gigasecond.do_not_create_datetime", SolutionCommentType.Essential);

    public static SolutionComment UseScientificNotationNotMathPow(string gigasecondValue) =>
        new("csharp.gigasecond.use_1e9_not_math_pow", SolutionCommentType.Informative, new SolutionCommentParameter(Value, gigasecondValue));

    public static SolutionComment UseScientificNotationOrDigitSeparators(string gigasecondValue) =>
        new("csharp.gigasecond.use_1e9_or_digit_separator", SolutionCommentType.Informative, new SolutionCommentParameter(Value, gigasecondValue));
}