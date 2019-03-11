using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.LeapSolutions;

namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class LeapAnalyzer
    {
        public static AnalyzedSolution Analyze(SolutionImplementation implementation)
        {
            if (implementation.IsEquivalentTo(MinimumNumberOfChecks))
                return implementation.ApproveAsOptimal();

            if (implementation.IsEquivalentTo(UnneededParentheses))
                return implementation.ApproveAsOptimal();

            if (implementation.IsEquivalentTo(MethodWithBlockBody))
                return implementation.ApproveWithComment("You could write the method an an expression-bodied member");

            if (implementation.UsesTooManyChecks())
                return implementation.DisapproveWithComment("Use minimum number of checks");

            return implementation.ReferToMentor();
        }

        private static bool UsesTooManyChecks(this SolutionImplementation implementation)
        {
            const int minimalNumberOfChecks = 3;

            var addMethod = implementation.Implementation.SyntaxNode
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