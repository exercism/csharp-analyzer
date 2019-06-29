using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedCommentParameters;

namespace Exercism.Analyzers.CSharp.Analyzers.Shared
{
    internal static class SharedComments
    {
        public static readonly SolutionComment RemoveMainMethod = new SolutionComment("csharp.general.has_main_method");
        public static readonly SolutionComment FixCompileErrors = new SolutionComment("csharp.general.has_compile_errors");
        public static readonly SolutionComment UseNullCoalescingOperatorNotNullCheck = new SolutionComment("csharp.general.use_null_coalescing_operator_not_null_check");
        public static readonly SolutionComment UseStringInterpolationNotStringFormat = new SolutionComment("csharp.general.use_string_interpolation_not_string_format");
        public static readonly SolutionComment UseStringInterpolationNotStringConcatenation = new SolutionComment("csharp.general.use_string_interpolation_not_string_concatenation");
        public static readonly SolutionComment RemoveThrowNotImplementedException = new SolutionComment("csharp.general.remove_throw_not_implemented_exception");
        public static readonly SolutionComment DoNotWriteToConsole = new SolutionComment("csharp.general.do_not_write_to_console");
        public static readonly SolutionComment ReturnImmediately = new SolutionComment("csharp.general.return_immediately");
        public static readonly SolutionComment ConvertFieldToConst = new SolutionComment("csharp.general.convert_field_to_const");
        public static readonly SolutionComment ConvertVariableToConst = new SolutionComment("csharp.general.convert_variable_to_const");
        public static readonly SolutionComment UsePrivateVisibility = new SolutionComment("csharp.general.use_private_visibility");
        public static readonly SolutionComment DoNotUseNestedIfStatement = new SolutionComment("csharp.general.do_not_use_nested_if_statement");
        
        public static SolutionComment UseExpressionBodiedMember(string method) =>
            new SolutionComment("csharp.general.use_expression_bodied_member", new SolutionCommentParameter(Name, method));
    }
}