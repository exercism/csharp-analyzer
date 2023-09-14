namespace Exercism.Analyzers.CSharp.Exercises;

internal class ExerciseAnalyzer
{
    public readonly Analysis Analysis = Analysis.Empty;
    
    public void Analyze(Solution solution)
    {
        AnalyzeCommon(solution);
        AnalyzeExerciseSpecific(solution);
    }

    private void AnalyzeCommon(Solution solution)
    {
        // TODO
    }

    protected virtual void AnalyzeExerciseSpecific(Solution solution)
    {
    }
}

// using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedCommentParameters;
//
// namespace Exercism.Analyzers.CSharp.Analyzers.Shared;
//
// internal static class SharedComments
// {
//     public static readonly SolutionComment HasMainMethod = new("csharp.general.has_main_method", SolutionCommentType.Essential);
//     public static readonly SolutionComment HasCompileErrors = new("csharp.general.has_compile_errors", SolutionCommentType.Essential);
//     public static readonly SolutionComment UseNullCoalescingOperatorNotNullCheck = new("csharp.general.use_null_coalescing_operator_not_null_check", SolutionCommentType.Actionable);
//     public static readonly SolutionComment UseStringInterpolationNotStringFormat = new("csharp.general.use_string_interpolation_not_string_format", SolutionCommentType.Actionable);
//     public static readonly SolutionComment UseStringInterpolationNotStringConcatenation = new("csharp.general.use_string_interpolation_not_string_concatenation", SolutionCommentType.Informative);
//     public static readonly SolutionComment RemoveThrowNotImplementedException = new("csharp.general.remove_throw_not_implemented_exception", SolutionCommentType.Essential);
//     public static readonly SolutionComment DoNotWriteToConsole = new("csharp.general.do_not_write_to_console", SolutionCommentType.Actionable);
//     public static readonly SolutionComment DoNotAssignAndReturn = new("csharp.general.do_not_assign_and_return", SolutionCommentType.Actionable);
//     public static readonly SolutionComment DoNotUseNestedIfStatement = new("csharp.general.do_not_use_nested_if_statement", SolutionCommentType.Actionable);
//
//     public static SolutionComment UsePrivateVisibility(string field) =>
//         new("csharp.general.use_private_visibility", SolutionCommentType.Actionable, new SolutionCommentParameter(Name, field));
//
//     public static SolutionComment ConvertFieldToConst(string field) =>
//         new("csharp.general.convert_field_to_const", SolutionCommentType.Actionable, new SolutionCommentParameter(Name, field));
//
//     public static SolutionComment ConvertVariableToConst(string variable) =>
//         new("csharp.general.convert_variable_to_const", SolutionCommentType.Actionable, new SolutionCommentParameter(Name, variable));
//
//     public static SolutionComment UseExpressionBodiedMember(string method) =>
//         new("csharp.general.use_expression_bodied_member", SolutionCommentType.Informative, new SolutionCommentParameter(Name, method));
//
//     public static SolutionComment MissingClass(string @class) =>
//         new("csharp.general.missing_class", SolutionCommentType.Essential, new SolutionCommentParameter(Name, @class));
//
//     public static SolutionComment MissingMethod(string method) =>
//         new("csharp.general.missing_method", SolutionCommentType.Essential, new SolutionCommentParameter(Name, method));
//
//     public static SolutionComment InvalidMethodSignature(string method, string signature) =>
//         new("csharp.general.invalid_method_signature", SolutionCommentType.Essential, new SolutionCommentParameter(Name, method), new SolutionCommentParameter(Signature, signature));
//
//     public static SolutionComment MissingProperty(string property) =>
//         new("csharp.general.missing_property", SolutionCommentType.Essential, new SolutionCommentParameter(Name, property));
//
//     public static SolutionComment PropertyIsNotAutoProperty(string name) =>
//         new("csharp.general.property_is_not_auto_property", SolutionCommentType.Actionable, new SolutionCommentParameter(Name, name));
//
//     public static SolutionComment PropertyHasNonPrivateSetter(string name) =>
//         new("csharp.general.property_setter_is_not_private", SolutionCommentType.Actionable, new SolutionCommentParameter(Name, name));
//
//     public static SolutionComment PropertyBetterUseInitializer(string name) =>
//         new("csharp.general.property_better_use_initializer", SolutionCommentType.Actionable, new SolutionCommentParameter(Name, name));
// }