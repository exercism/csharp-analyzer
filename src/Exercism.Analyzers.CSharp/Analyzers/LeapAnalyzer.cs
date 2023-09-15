using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal class LeapAnalyzer : Analyzer
{
    public LeapAnalyzer(Compilation compilation, Analysis analysis) : base(compilation, analysis) { }

    private static class Comments
    {
        public static readonly Comment DoNotUseIsLeapYear =
            new("csharp.leap.do_not_use_is_leap_year", CommentType.Essential);

        public static readonly Comment DoNotUseIfStatement =
            new("csharp.leap.do_not_use_if_statement", CommentType.Actionable);

        public static readonly Comment UseMinimumNumberOfChecks =
            new("csharp.leap.use_minimum_number_of_checks", CommentType.Actionable);
    }
}

// using System.Linq;
//
// using Exercism.Analyzers.CSharp.Syntax;
// using Exercism.Analyzers.CSharp.Syntax.Comparison;
//
// using Microsoft.CodeAnalysis;
// using Microsoft.CodeAnalysis.CSharp;
// using Microsoft.CodeAnalysis.CSharp.Syntax;
//
// using static Exercism.Analyzers.CSharp.Analyzers.Leap.LeapSyntaxFactory;
// using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
//
// namespace Exercism.Analyzers.CSharp.Analyzers.Leap;
//
// internal class LeapSolution : Solution
// {
//     public const string LeapClassName = "Leap";
//     public const string IsLeapYearMethodName = "IsLeapYear";
//     public const string IsLeapYearMethodSignature = "public static bool IsLeapYear(int year)";
//
//     private const int MinimalNumberOfChecks = 3;
//
//     public LeapSolution(Solution solution) : base(solution)
//     {
//     }
//
//     private ClassDeclarationSyntax LeapClass =>
//         SyntaxRoot.GetClass(LeapClassName);
//
//     private MethodDeclarationSyntax IsLeapYearMethod =>
//         LeapClass?.GetMethod(IsLeapYearMethodName);
//
//     public bool MissingLeapClass =>
//         LeapClass == null;
//
//     public bool MissingIsLeapYearMethod =>
//         !MissingLeapClass && IsLeapYearMethod == null;
//
//     // TODO: create helper method to verify type signature
//     // TODO: create helper method for predefined types
//     public bool InvalidIsLeapYearMethod =>
//         IsLeapYearMethod != null &&
//         (IsLeapYearMethod.ParameterList.Parameters.Count != 1 ||
//          !IsLeapYearMethod.ParameterList.Parameters[0].Type.IsEquivalentWhenNormalized(
//              PredefinedType(
//                  Token(SyntaxKind.IntKeyword))) ||
//          !IsLeapYearMethod.ReturnType.IsEquivalentWhenNormalized(
//              PredefinedType(
//                  Token(SyntaxKind.BoolKeyword))));
//
//     private ParameterSyntax YearParameter =>
//         IsLeapYearMethod?.ParameterList.Parameters.FirstOrDefault();
//
//     private ExpressionSyntax ReturnedExpression =>
//         IsLeapYearMethod?.ReturnedExpression();
//
//     public string YearParameterName =>
//         YearParameter?.Identifier.Text;
//
//     public bool UsesDateTimeIsLeapYear =>
//         IsLeapYearMethod.InvokesMethod(IsLeapYearMemberAccessExpression());
//
//     public bool CanUseExpressionBody =>
//         IsLeapYearMethod.CanUseExpressionBody();
//
//     public bool UsesExpressionBody =>
//         IsLeapYearMethod.IsExpressionBody();
//
//     public bool UsesTooManyChecks =>
//         IsLeapYearMethod
//             .DescendantNodes<BinaryExpressionSyntax>()
//             .Count(BinaryExpressionUsesYearParameter) > MinimalNumberOfChecks;
//
//     public bool UsesIfStatement =>
//         IsLeapYearMethod.UsesIfStatement();
//
//     public bool UsesNestedIfStatement =>
//         IsLeapYearMethod.UsesNestedIfStatement();
//
//     public bool ReturnsMinimumNumberOfChecksInSingleExpression =>
//         Returns(LeapMinimumNumberOfChecksWithoutParenthesesBinaryExpression(this)) ||
//         Returns(LeapMinimumNumberOfChecksWithoutParenthesesBinaryExpressionReversed(this)) ||
//         Returns(LeapMinimumNumberOfChecksWithParenthesesBinaryExpression(this));
//
//     private bool BinaryExpressionUsesYearParameter(BinaryExpressionSyntax binaryExpression) =>
//         binaryExpression.Left.IsEquivalentWhenNormalized(
//             LeapParameterIdentifierName(this)) ||
//         binaryExpression.Right.IsEquivalentWhenNormalized(
//             LeapParameterIdentifierName(this));
//
//     private bool Returns(SyntaxNode returned) =>
//         ReturnedExpression.IsEquivalentWhenNormalized(returned);
// }
//
// namespace Exercism.Analyzers.CSharp.Analyzers.Leap;
//

//
// using static Exercism.Analyzers.CSharp.Analyzers.Leap.LeapComments;
// using static Exercism.Analyzers.CSharp.Analyzers.Leap.LeapSolution;
// using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;
//
// namespace Exercism.Analyzers.CSharp.Analyzers.Leap;
//
// internal class LeapAnalyzer : ExerciseAnalyzer<LeapSolution>
// {
//     protected override SolutionAnalysis AnalyzeSpecific(LeapSolution solution)
//     {
//         if (solution.MissingLeapClass)
//             return AnalysisWithComment(MissingClass(LeapClassName));
//
//         if (solution.MissingIsLeapYearMethod)
//             return AnalysisWithComment(MissingMethod(IsLeapYearMethodName));
//
//         if (solution.InvalidIsLeapYearMethod)
//             return AnalysisWithComment(InvalidMethodSignature(IsLeapYearMethodName, IsLeapYearMethodSignature));
//
//         if (solution.UsesDateTimeIsLeapYear)
//             AddComment(DoNotUseIsLeapYear);
//
//         if (solution.UsesNestedIfStatement)
//             AddComment(DoNotUseNestedIfStatement);
//         else if (solution.UsesIfStatement)
//             AddComment(DoNotUseIfStatement);
//
//         if (solution.UsesTooManyChecks)
//             AddComment(UseMinimumNumberOfChecks);
//
//         if (solution.CanUseExpressionBody)
//             AddComment(UseExpressionBodiedMember(IsLeapYearMethodName));
//
//         return Analysis;
//     }
// }