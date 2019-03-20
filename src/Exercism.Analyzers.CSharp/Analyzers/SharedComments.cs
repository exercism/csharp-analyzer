namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class SharedComments
    {
        public const string HasMainMethod = "csharp.general.has_main_method";
        public const string HasCompileErrors = "csharp.general.has_compile_errors";
        public const string UseExpressionBodiedMember = "csharp.general.use_expression_bodied_member";
        public const string UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck = "csharp.general.use_null_coalescing_operator_not_ternary_operator_with_null_check";
        public const string UseStringInterpolationNotStringFormat = "csharp.general.use_string_interpolation_not_string_format";
        public const string UseStringInterpolationNotStringConcatenation = "csharp.general.use_string_interpolation_not_string_concatenation";
        public const string DontAssignToParameter = "csharp.general.dont_assign_to_parameter";
    }
}