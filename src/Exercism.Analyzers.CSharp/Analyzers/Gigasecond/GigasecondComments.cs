namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal static class GigasecondComments
    {
        public static readonly SolutionComment UseAddSeconds = new SolutionComment("csharp.gigasecond.use_add_seconds");
        public static readonly SolutionComment UseScientificNotationNotMathPow = new SolutionComment("csharp.gigasecond.use_1e9_not_math_pow");
        public static readonly SolutionComment UseScientificNotationOrDigitSeparators = new SolutionComment("csharp.gigasecond.use_1e9_or_digit_separator");
        public static readonly SolutionComment DoNotCreateDateTime = new SolutionComment("csharp.gigasecond.do_not_create_datetime");
    }
}