using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Leap
{
    internal static class LeapSyntaxFactory
    {
        public static IdentifierNameSyntax LeapParameterIdentifierName(LeapSolution leapSolution) =>
            IdentifierName(leapSolution.YearParameterName);

        public static BinaryExpressionSyntax LeapMinimumNumberOfChecksWithoutParenthesesBinaryExpression(LeapSolution leapSolution) =>
            LogicalOrExpression(
                LogicalAndExpression(
                    DivisibleByFour(leapSolution),
                    NotDivisibleByHundred(leapSolution)),
                DivisibleByFourHundred(leapSolution));

        public static BinaryExpressionSyntax LeapMinimumNumberOfChecksWithoutParenthesesBinaryExpressionReversed(LeapSolution leapSolution) =>
            LogicalOrExpression(
                DivisibleByFourHundred(leapSolution),
                LogicalAndExpression(
                    NotDivisibleByHundred(leapSolution),
                    DivisibleByFour(leapSolution)));

        public static BinaryExpressionSyntax LeapMinimumNumberOfChecksWithParenthesesBinaryExpression(LeapSolution leapSolution) =>
            LogicalAndExpression(
                DivisibleByFour(leapSolution),
                ParenthesizedExpression(
                    LogicalOrExpression(
                        NotDivisibleByHundred(leapSolution),
                        DivisibleByFourHundred(leapSolution))));

        private static BinaryExpressionSyntax DivisibleByFour(LeapSolution leapSolution) =>
            EqualsExpression(
                LeapModuloExpression(leapSolution, 4),
                NumericLiteralExpression(0));

        private static BinaryExpressionSyntax NotDivisibleByHundred(LeapSolution leapSolution) =>
            NotEqualsExpression(
                LeapModuloExpression(leapSolution, 100),
                NumericLiteralExpression(0));

        private static BinaryExpressionSyntax DivisibleByFourHundred(LeapSolution leapSolution) =>
            EqualsExpression(
                LeapModuloExpression(leapSolution, 400),
                NumericLiteralExpression(0));

        private static BinaryExpressionSyntax LeapModuloExpression(LeapSolution leapSolution, int year) =>
            ModuloExpression(
                LeapParameterIdentifierName(leapSolution),
                NumericLiteralExpression(year));
    }
}