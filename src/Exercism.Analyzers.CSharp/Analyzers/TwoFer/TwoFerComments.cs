namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal static class TwoFerComments
    {
        public const string UseSingleFormattedStringNotMultiple = "csharp.two-fer.use_single_formatted_string_not_multiple";
        public const string UseStringInterpolationNotStringReplace = "csharp.two-fer.use_string_interpolation_not_string_replace";
        public const string UseStringInterpolationNotStringJoin = "csharp.two-fer.use_string_interpolation_not_string_join";
        public const string UseStringInterpolationNotStringConcat = "csharp.two-fer.use_string_interpolation_not_string_concat";
        public const string UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrEmptyCheck = "csharp.two-fer.use_null_coalescing_operator_not_ternary_operator_with_is_null_or_empty";
        public const string UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrWhiteSpaceCheck = "csharp.two-fer.use_null_coalescing_operator_not_ternary_operator_with_is_null_or_white_space";
        public const string UseNullCoalescingOperatorNotIsNullOrEmptyCheck = "csharp.two-fer.use_null_coalescing_operator_not_is_null_or_empty";
        public const string UseNullCoalescingOperatorNotIsNullOrWhiteSpaceCheck = "csharp.two-fer.use_null_coalescing_operator_not_is_null_or_white_space";
        public const string UseDefaultValue = "csharp.two-fer.use_default_value";
        public const string InvalidDefaultValue = "csharp.two-fer.invalid_default_value";
    }
}