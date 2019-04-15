namespace Exercism.Analyzers.CSharp.Analyzers.Shared
{
    internal static class SharedComments
    {
        public const string RemoveMainMethod = "csharp.general.has_main_method";
        public const string FixCompileErrors = "csharp.general.has_compile_errors";
        public const string UseExpressionBodiedMember = "csharp.general.use_expression_bodied_member";
        public const string UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck = "csharp.general.use_null_coalescing_operator_not_ternary_operator_with_null_check";
        public const string UseNullCoalescingOperatorNotNullCheck = "csharp.general.use_null_coalescing_operator_not_null_check";
        public const string UseStringInterpolationNotStringFormat = "csharp.general.use_string_interpolation_not_string_format";
        public const string UseStringInterpolationNotStringConcatenation = "csharp.general.use_string_interpolation_not_string_concatenation";
        public const string RemoveThrowNotImplementedException = "csharp.general.remove_throw_not_implemented_exception";
        public const string DontWriteToConsole = "csharp.general.dont_write_to_console";
        public const string InlineVariable = "csharp.general.inline_variable";
        public const string InlineParameter = "csharp.general.inline_parameter";
    }
}