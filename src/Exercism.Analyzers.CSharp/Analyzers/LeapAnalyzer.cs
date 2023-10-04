using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal class LeapAnalyzer : Analyzer
{
    public LeapAnalyzer(Submission submission) : base(submission)
    {
    }

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        if (GetDeclaredSymbolName(node) == "Leap.IsLeapYear(int)" && 
            GetDeclaredSymbol(node.ParameterList.Parameters[0]) is { } parameterSymbol)
        {
            if (node.DescendantNodes()
                    .OfType<IdentifierNameSyntax>()
                    .Select(GetSymbol)
                    .Count(symbol => parameterSymbol.Equals(symbol, SymbolEqualityComparer.Default)) > 3)
                AddComment(Comments.UseMinimumNumberOfChecks);
        }

        base.VisitMethodDeclaration(node);
    }

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        var symbolName = GetSymbolName(node);
        switch (symbolName)
        {
            case "System.DateTime.IsLeapYear(int)":
                AddComment(Comments.DoNotUseIsLeapYear);
                AddTags(Tags.UsesDateTimeIsLeapYear);
                break;
            case "System.DateTime.AddDays(double)":
                AddTags(Tags.UsesDateTimeAddDays);
                break;
        }

        base.VisitInvocationExpression(node);
    }

    public override void VisitIfStatement(IfStatementSyntax node)
    {
        AddComment(Comments.DoNotUseIfStatement);
        base.VisitIfStatement(node);
    }

    private static class Comments
    {
        public static readonly Comment DoNotUseIsLeapYear =
            new("csharp.leap.do_not_use_is_leap_year", CommentType.Essential);

        public static readonly Comment DoNotUseIfStatement =
            new("csharp.leap.do_not_use_if_statement", CommentType.Actionable);

        public static readonly Comment UseMinimumNumberOfChecks =
            new("csharp.leap.use_minimum_number_of_checks", CommentType.Actionable);
    }

    private static class Tags
    {
        public const string UsesDateTimeIsLeapYear = "uses:DateTime.IsLeapYear";
        public const string UsesDateTimeAddDays = "uses:DateTime.AddDays";
    }
}
