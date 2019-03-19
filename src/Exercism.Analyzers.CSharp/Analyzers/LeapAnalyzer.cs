using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.LeapSolutions;
using static Exercism.Analyzers.CSharp.Analyzers.LeapComments;
using static Exercism.Analyzers.CSharp.Analyzers.DefaultComments;

namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class LeapAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution)
        {
            if (parsedSolution.IsEquivalentTo(MinimumNumberOfChecksInExpressionBody) ||
                parsedSolution.IsEquivalentTo(MinimumNumberOfChecksWithParenthesesInExpressionBody))
                return parsedSolution.ApproveAsOptimal();

            if (parsedSolution.IsEquivalentTo(MinimumNumberOfChecksInBlockBody) ||
                parsedSolution.IsEquivalentTo(MinimumNumberOfChecksWithParenthesesInBlockBody))
                return parsedSolution.ApproveWithComment(UseExpressionBodiedMember);

            if (parsedSolution.UsesTooManyChecks())
                return parsedSolution.DisapproveWithComment(UseMinimumNumberOfChecks);

            return parsedSolution.ReferToMentor();
        }

        private static bool UsesTooManyChecks(this ParsedSolution parsedSolution)
        {
            const int minimalNumberOfChecks = 3;

            var addMethod = parsedSolution.SyntaxRoot
                .GetClass("Leap")
                .GetMethod("IsLeapYear");

            return addMethod
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .Count(BinaryExpressionUsesYearParameter) > minimalNumberOfChecks;

            bool BinaryExpressionUsesYearParameter(BinaryExpressionSyntax binaryExpression) =>
                ExpressionUsesYearParameter(binaryExpression.Left) ||
                ExpressionUsesYearParameter(binaryExpression.Right);

            bool ExpressionUsesYearParameter(ExpressionSyntax expression) =>
                expression is IdentifierNameSyntax nameSyntax &&
                nameSyntax.Identifier.Text == "year";
        }
    }
}