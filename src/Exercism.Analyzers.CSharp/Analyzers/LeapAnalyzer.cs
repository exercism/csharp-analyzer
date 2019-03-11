using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.LeapSolutions;

namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class LeapAnalyzer
    {
        public static AnalyzedSolution Analyze(SolutionImplementation solutionImplementation)
        {
            if (solutionImplementation.IsEquivalentTo(MinimumNumberOfChecks))
                return new AnalyzedSolution(solutionImplementation.Solution, SolutionStatus.Approve);

            if (solutionImplementation.IsEquivalentTo(UnneededParentheses))
                return new AnalyzedSolution(solutionImplementation.Solution, SolutionStatus.Approve);

            if (solutionImplementation.IsEquivalentTo(MethodWithBlockBody))
                return new AnalyzedSolution(solutionImplementation.Solution, SolutionStatus.Approve, "You could write the method an an expression-bodied member");

            if (solutionImplementation.UsesTooManyChecks())
                return new AnalyzedSolution(solutionImplementation.Solution, SolutionStatus.ReferToMentor, "Use minimum number of checks");

            return new AnalyzedSolution(solutionImplementation.Solution, SolutionStatus.ReferToMentor);
        }

        private static bool UsesTooManyChecks(this SolutionImplementation solutionImplementation)
        {
            const int minimalNumberOfChecks = 3;

            var addMethod = solutionImplementation.SyntaxNode
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