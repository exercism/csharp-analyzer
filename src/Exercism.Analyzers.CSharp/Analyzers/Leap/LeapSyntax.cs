using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Exercism.Analyzers.CSharp.Analyzers.Syntax.Comparison;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Leap.LeapSyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Leap
{
    internal static class LeapSyntax
    {
        private const int MinimalNumberOfChecks = 3;

        public static bool ReturnsMinimumNumberOfChecksInSingleExpression(this LeapSolution leapSolution) =>
            leapSolution.Returns(LeapMinimumNumberOfChecksWithoutParenthesesBinaryExpression(leapSolution)) ||
            leapSolution.Returns(LeapMinimumNumberOfChecksWithParenthesesBinaryExpression(leapSolution));

        public static bool UsesExpressionBody(this LeapSolution leapSolution) =>
            leapSolution.IsLeapYearMethod.IsExpressionBody();
        
        public static bool UsesTooManyChecks(this LeapSolution leapSolution) =>
            leapSolution.IsLeapYearMethod
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .Count(leapSolution.BinaryExpressionUsesYearParameter) > MinimalNumberOfChecks;

        private static bool BinaryExpressionUsesYearParameter(this LeapSolution leapSolution, BinaryExpressionSyntax binaryExpression) =>
            binaryExpression.Left.IsEquivalentWhenNormalized(
                LeapParameterIdentifierName(leapSolution)) ||
            binaryExpression.Right.IsEquivalentWhenNormalized(
                LeapParameterIdentifierName(leapSolution));
    }
}