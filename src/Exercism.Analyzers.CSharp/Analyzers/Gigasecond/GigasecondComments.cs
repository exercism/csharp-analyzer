using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedCommentParameters;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal static class GigasecondComments
    {
        public static readonly SolutionComment UseAddSeconds = new SolutionComment("csharp.gigasecond.use_add_seconds");
        public static readonly SolutionComment DoNotCreateDateTime = new SolutionComment("csharp.gigasecond.do_not_create_datetime");

        public static SolutionComment UseScientificNotationNotMathPow(string gigasecondValue) =>
            new SolutionComment("csharp.gigasecond.use_1e9_not_math_pow", new SolutionCommentParameter(Value, gigasecondValue));

        public static SolutionComment UseScientificNotationOrDigitSeparators(string gigasecondValue) =>
            new SolutionComment("csharp.gigasecond.use_1e9_or_digit_separator", new SolutionCommentParameter(Value, gigasecondValue));
    }
}