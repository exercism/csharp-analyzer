using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.LeapSolutions;

namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class LeapAnalyzer
    {
        public static SolutionAnalysis Analyze(SolutionImplementation solution)
        {
            if (solution.IsEquivalentTo(MinimumNumberOfChecks))
                return solution.ApproveAsOptimal();

            if (solution.IsEquivalentTo(UnneededParentheses))
                return solution.ApproveAsOptimal();

            if (solution.IsEquivalentTo(MethodWithBlockBody))
                return solution.ApproveWithComment("You could write the method an an expression-bodied member");

            if (solution.UsesTooManyChecks())
                return solution.DisapproveWithComment("Use minimum number of checks");

            return solution.ReferToMentor();
        }

        private static bool UsesTooManyChecks(this SolutionImplementation solution)
        {
            const int minimalNumberOfChecks = 3;

            var addMethod = solution.Implementation.SyntaxNode
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