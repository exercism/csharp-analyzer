using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal class TwoFerAnalyzer : Analyzer
{
    public TwoFerAnalyzer(Submission submission) : base(submission)
    {
    }

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        switch (GetDeclaredSymbolName(node))
        {
            case "TwoFer.Speak()":
                AddComment(Comments.UseDefaultValueNotOverloads);
                break;
            case "TwoFer.Speak(string)":
            {
                var parameter = node.ParameterList.Parameters[0];
                var parameterName = parameter.Identifier.Text;
            
                if (parameter is { Default: null })
                    AddComment(Comments.UseDefaultValue(parameterName));
                else if (parameter is { Default.Value: LiteralExpressionSyntax { Token: { Text: {} defaultValue and not "\"you\"" } } })
                    AddComment(Comments.InvalidDefaultValue(parameterName, defaultValue));
                break;
            }
        }

        base.VisitMethodDeclaration(node);
    }
    
    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        switch (GetSymbolName(node))
        {
            case "string.Concat(string?, string?, string?)":
                AddComment(Comments.UseStringInterpolationNotStringConcat);
                break;
            case "string.IsNullOrEmpty(string?)":
                AddComment(Comments.UseNullCoalescingOperatorNotIsNullOrEmptyCheck);
                break;
            case "string.IsNullOrWhiteSpace(string?)":
                AddComment(Comments.UseNullCoalescingOperatorNotIsNullOrWhiteSpaceCheck);
                break;
        }

        base.VisitInvocationExpression(node);
    }
    
    private static class Comments
    {
        public static readonly Comment UseSingleFormattedStringNotMultiple =
            new("csharp.two-fer.use_single_formatted_string_not_multiple", CommentType.Essential);

        public static readonly Comment UseStringInterpolationNotStringReplace =
            new("csharp.two-fer.use_string_interpolation_not_string_replace", CommentType.Actionable);

        public static readonly Comment UseStringInterpolationNotStringJoin =
            new("csharp.two-fer.use_string_interpolation_not_string_join", CommentType.Actionable);

        public static readonly Comment UseStringInterpolationNotStringConcat =
            new("csharp.two-fer.use_string_interpolation_not_string_concat", CommentType.Actionable);

        public static readonly Comment UseNullCoalescingOperatorNotIsNullOrEmptyCheck =
            new("csharp.two-fer.use_null_coalescing_operator_not_is_null_or_empty", CommentType.Actionable);

        public static readonly Comment UseNullCoalescingOperatorNotIsNullOrWhiteSpaceCheck =
            new("csharp.two-fer.use_null_coalescing_operator_not_is_null_or_white_space", CommentType.Actionable);

        public static readonly Comment UseDefaultValueNotOverloads =
            new("csharp.two-fer.use_default_value_not_overloads", CommentType.Essential);

        public static Comment UseDefaultValue(string parameterName) =>
            new("csharp.two-fer.use_default_value", CommentType.Essential, new CommentParameter("name", parameterName));

        public static Comment InvalidDefaultValue(string parameterName, string defaultValue) =>
            new("csharp.two-fer.invalid_default_value", CommentType.Actionable,
                new CommentParameter("name", parameterName), new CommentParameter("value", defaultValue));
    }
}
