using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedCommentParameters;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal static class TwoFerComments
    {
        public static readonly SolutionComment UseSingleFormattedStringNotMultiple = new SolutionComment("csharp.two-fer.use_single_formatted_string_not_multiple");
        public static readonly SolutionComment UseStringInterpolationNotStringReplace = new SolutionComment("csharp.two-fer.use_string_interpolation_not_string_replace");
        public static readonly SolutionComment UseStringInterpolationNotStringJoin = new SolutionComment("csharp.two-fer.use_string_interpolation_not_string_join");
        public static readonly SolutionComment UseStringInterpolationNotStringConcat = new SolutionComment("csharp.two-fer.use_string_interpolation_not_string_concat");
        public static readonly SolutionComment UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrEmptyCheck = new SolutionComment("csharp.two-fer.use_null_coalescing_operator_not_ternary_operator_with_is_null_or_empty");
        public static readonly SolutionComment UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrWhiteSpaceCheck = new SolutionComment("csharp.two-fer.use_null_coalescing_operator_not_ternary_operator_with_is_null_or_white_space");
        public static readonly SolutionComment UseNullCoalescingOperatorNotIsNullOrEmptyCheck = new SolutionComment("csharp.two-fer.use_null_coalescing_operator_not_is_null_or_empty");
        public static readonly SolutionComment UseNullCoalescingOperatorNotIsNullOrWhiteSpaceCheck = new SolutionComment("csharp.two-fer.use_null_coalescing_operator_not_is_null_or_white_space");
        public static readonly SolutionComment UseDefaultValue = new SolutionComment("csharp.two-fer.use_default_value");
        public static SolutionComment InvalidDefaultValue(string parameterName, string defaultValue) =>
            new SolutionComment("csharp.two-fer.invalid_default_value", new SolutionCommentParameter(Name, parameterName), new SolutionCommentParameter(Value, defaultValue));
    }
}