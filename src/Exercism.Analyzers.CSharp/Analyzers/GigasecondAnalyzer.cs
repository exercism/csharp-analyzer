using System.Linq;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal class GigasecondAnalyzer : Analyzer
{
    public GigasecondAnalyzer(Submission submission) : base(submission)
    {
    }

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        var symbol = SemanticModel.GetSymbolInfo(node).Symbol;
        switch (symbol?.ToString())
        {
            case "System.DateTime.AddSeconds(double)":
                AddComment(Comments.UseAddSeconds);

                var argument = node.ArgumentList.Arguments[0].Expression.ToString();
                if (argument.All(char.IsDigit))
                    AddComment(Comments.UseScientificNotationOrDigitSeparators(argument));
                
                AddTags(Tags.UsesDateTimeAddSeconds);
                break;
            case "System.DateTime.AddDays(double)":
                AddComment(Comments.UseAddSeconds);
                AddTags(Tags.UsesDateTimeAddDays);
                break;
            case "System.Math.Pow(double, double)" when node.ArgumentList.ToString() == "(10,9)":
                AddComment(Comments.UseScientificNotationNotMathPow(node.ToString()));
                AddTags(Tags.UsesMathPow);
                break;
        }

        base.VisitInvocationExpression(node);
    }

    public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
    {
        var symbol = SemanticModel.GetSymbolInfo(node);

        if (symbol.ToString() == "System.DateTime.DateTime(long)")
        {
            AddComment(Comments.DoNotCreateDateTime);
            AddTags(Tags.UsesDateTimeConstructor);
        }

        base.VisitObjectCreationExpression(node);
    }

    public override void VisitBinaryExpression(BinaryExpressionSyntax node)
    {
        if (SemanticModel.GetSymbolInfo(node).Symbol?.ToString() == "System.DateTime.operator +(System.DateTime, System.TimeSpan)")
            AddTags(Tags.UsesDateTimePlusTimeSpan);

        base.VisitBinaryExpression(node);
    }

    private static class Comments
    {
        public static readonly Comment UseAddSeconds = new("csharp.gigasecond.use_add_seconds", CommentType.Actionable);

        public static readonly Comment DoNotCreateDateTime =
            new("csharp.gigasecond.do_not_create_datetime", CommentType.Essential);

        public static Comment UseScientificNotationNotMathPow(string currentValue) =>
            new("csharp.gigasecond.use_1e9_not_math_pow", CommentType.Informative,
                new CommentParameter("value", currentValue));

        public static Comment UseScientificNotationOrDigitSeparators(string currentValue) =>
            new("csharp.gigasecond.use_1e9_or_digit_separator", CommentType.Informative,
                new CommentParameter("value", currentValue));
    }

    private static class Tags
    {
        public const string UsesDateTimeConstructor = "uses:DateTime.DateTime(long)";
        public const string UsesDateTimeAddDays = "uses:DateTime.AddDays";
        public const string UsesDateTimeAddSeconds = "uses:DateTime.AddSeconds";
        public const string UsesDateTimePlusTimeSpan = "uses:DateTime.Plus(TimeSpan)";
        public const string UsesMathPow = "uses:Math.Pow";
    }
}