using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedCommentParameters;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer;

internal static class TwoFerComments
{
    public static readonly SolutionComment UseSingleFormattedStringNotMultiple = new("csharp.two-fer.use_single_formatted_string_not_multiple", SolutionCommentType.Essential);
    public static readonly SolutionComment UseStringInterpolationNotStringReplace = new("csharp.two-fer.use_string_interpolation_not_string_replace", SolutionCommentType.Actionable);
    public static readonly SolutionComment UseStringInterpolationNotStringJoin = new("csharp.two-fer.use_string_interpolation_not_string_join", SolutionCommentType.Actionable);
    public static readonly SolutionComment UseStringInterpolationNotStringConcat = new("csharp.two-fer.use_string_interpolation_not_string_concat", SolutionCommentType.Actionable);
    public static readonly SolutionComment UseNullCoalescingOperatorNotIsNullOrEmptyCheck = new("csharp.two-fer.use_null_coalescing_operator_not_is_null_or_empty", SolutionCommentType.Actionable);
    public static readonly SolutionComment UseNullCoalescingOperatorNotIsNullOrWhiteSpaceCheck = new("csharp.two-fer.use_null_coalescing_operator_not_is_null_or_white_space", SolutionCommentType.Actionable);
    public static readonly SolutionComment UseDefaultValueNotOverloads = new("csharp.two-fer.use_default_value_not_overloads", SolutionCommentType.Essential);

    public static SolutionComment UseDefaultValue(string parameterName) =>
        new("csharp.two-fer.use_default_value", SolutionCommentType.Essential, new SolutionCommentParameter(Name, parameterName));

    public static SolutionComment InvalidDefaultValue(string parameterName, string defaultValue) =>
        new("csharp.two-fer.invalid_default_value", SolutionCommentType.Actionable, new SolutionCommentParameter(Name, parameterName), new SolutionCommentParameter(Value, defaultValue));
}