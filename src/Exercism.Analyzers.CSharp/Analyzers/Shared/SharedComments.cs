using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedCommentParameters;

namespace Exercism.Analyzers.CSharp.Analyzers.Shared
{
    internal static class SharedComments
    {
        public static readonly SolutionComment HasMainMethod = new SolutionComment("csharp.general.has_main_method");
        public static readonly SolutionComment HasCompileErrors = new SolutionComment("csharp.general.has_compile_errors");
        public static readonly SolutionComment UseNullCoalescingOperatorNotNullCheck = new SolutionComment("csharp.general.use_null_coalescing_operator_not_null_check");
        public static readonly SolutionComment UseStringInterpolationNotStringFormat = new SolutionComment("csharp.general.use_string_interpolation_not_string_format");
        public static readonly SolutionComment UseStringInterpolationNotStringConcatenation = new SolutionComment("csharp.general.use_string_interpolation_not_string_concatenation");
        public static readonly SolutionComment RemoveThrowNotImplementedException = new SolutionComment("csharp.general.remove_throw_not_implemented_exception");
        public static readonly SolutionComment DoNotWriteToConsole = new SolutionComment("csharp.general.do_not_write_to_console");
        public static readonly SolutionComment DoNotAssignAndReturn = new SolutionComment("csharp.general.do_not_assign_and_return");
        public static readonly SolutionComment DoNotUseNestedIfStatement = new SolutionComment("csharp.general.do_not_use_nested_if_statement");

        public static SolutionComment UsePrivateVisibility(string field) =>
            new SolutionComment("csharp.general.use_private_visibility", new SolutionCommentParameter(Name, field));

        public static SolutionComment ConvertFieldToConst(string field) =>
            new SolutionComment("csharp.general.convert_field_to_const", new SolutionCommentParameter(Name, field));

        public static SolutionComment ConvertVariableToConst(string variable) =>
            new SolutionComment("csharp.general.convert_variable_to_const", new SolutionCommentParameter(Name, variable));

        public static SolutionComment UseExpressionBodiedMember(string method) =>
            new SolutionComment("csharp.general.use_expression_bodied_member", new SolutionCommentParameter(Name, method));

        public static SolutionComment MissingClass(string @class) =>
            new SolutionComment("csharp.general.missing_class", new SolutionCommentParameter(Name, @class));

        public static SolutionComment MissingMethod(string method) =>
            new SolutionComment("csharp.general.missing_method", new SolutionCommentParameter(Name, method));

        public static SolutionComment InvalidMethodSignature(string method, string signature) =>
            new SolutionComment("csharp.general.invalid_method_signature", new SolutionCommentParameter(Name, method), new SolutionCommentParameter(Signature, signature));

        public static SolutionComment MissingProperty(string property) =>
            new("csharp.general.missing_property", new SolutionCommentParameter(Name, property));

        public static SolutionComment PropertyIsNotAutoProperty(string name) =>
            new("csharp.general.property_is_not_auto_property", new SolutionCommentParameter(Name, name));

        public static SolutionComment PrecisionPropertyHasNonPrivateSetter(string name) =>
            new("csharp.general.property_setter_is_not_private", new SolutionCommentParameter(Name, name));

        public static SolutionComment PropertyBetterUseInitializer(string name) =>
            new("csharp.general.property_better_use_initializer", new SolutionCommentParameter(Name, name));
    }
}