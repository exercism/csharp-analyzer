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
        public const string DoNotWriteToConsole = "csharp.general.do_not_write_to_console";
        public const string ReturnImmediately = "csharp.general.return_immediately";
        public const string UseConstant = "csharp.general.use_constant";
        public const string UsePrivateVisibility = "csharp.general.use_private_visibility";
        public const string DoNotUseNestedIfStatement = "csharp.general.do_not_use_nested_if_statement";
    }
}