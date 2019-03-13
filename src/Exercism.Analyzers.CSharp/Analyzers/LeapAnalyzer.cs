using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.LeapSolutions;

namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class LeapAnalyzer
    {
        public static SolutionAnalysis Analyze(CompiledSolution compiledSolution)
        {
            if (compiledSolution.IsEquivalentTo(MinimumNumberOfChecksInExpressionBody))
                return compiledSolution.ApproveAsOptimal();

            if (compiledSolution.IsEquivalentTo(MinimumNumberOfChecksInBlockBody))
                return compiledSolution.ApproveWithComment("You could write the method an an expression-bodied member");

            if (compiledSolution.IsEquivalentTo(MinimumNumberOfChecksWithParenthesesInExpressionBody))
                return compiledSolution.ApproveAsOptimal();

            if (compiledSolution.IsEquivalentTo(MinimumNumberOfChecksWithParenthesesInBlockBody))
                return compiledSolution.ApproveWithComment("You could write the method an an expression-bodied member");

            if (compiledSolution.UsesTooManyChecks())
                return compiledSolution.DisapproveWithComment("Use minimum number of checks");

            return compiledSolution.ReferToMentor();
        }

        private static bool UsesTooManyChecks(this CompiledSolution compiledSolution)
        {
            const int minimalNumberOfChecks = 3;

            var addMethod = compiledSolution.Implementation.SyntaxNode
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