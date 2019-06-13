using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Leap
{
    internal static class LeapSyntaxFactory
    {
        public static IdentifierNameSyntax LeapParameterIdentifierName(LeapSolution leapSolution) =>
            IdentifierName(leapSolution.YearParameter.Identifier);

        public static BinaryExpressionSyntax LeapMinimumNumberOfChecksWithoutParenthesesBinaryExpression(LeapSolution leapSolution) =>
            LogicalOrExpression(
                LogicalAndExpression(
                    DivisbleByFour(leapSolution),
                    NotDivisbleByHundred(leapSolution)),
                DivisbleByFourHundred(leapSolution));

        public static BinaryExpressionSyntax LeapMinimumNumberOfChecksWithoutParenthesesBinaryExpressionReversed(LeapSolution leapSolution) =>
            LogicalOrExpression(
                DivisbleByFourHundred(leapSolution),
                LogicalAndExpression(
                    NotDivisbleByHundred(leapSolution),
                    DivisbleByFour(leapSolution)));

        public static BinaryExpressionSyntax LeapMinimumNumberOfChecksWithParenthesesBinaryExpression(LeapSolution leapSolution) =>
            LogicalAndExpression(
                DivisbleByFour(leapSolution),
                ParenthesizedExpression(
                    LogicalOrExpression(
                        NotDivisbleByHundred(leapSolution),
                        DivisbleByFourHundred(leapSolution))));

        private static BinaryExpressionSyntax DivisbleByFour(LeapSolution leapSolution) =>
            EqualsExpression(
                LeapModuloExpression(leapSolution, 4),
                NumericLiteralExpression(0));

        private static BinaryExpressionSyntax NotDivisbleByHundred(LeapSolution leapSolution) =>
            NotEqualsExpression(
                LeapModuloExpression(leapSolution, 100),
                NumericLiteralExpression(0));

        private static BinaryExpressionSyntax DivisbleByFourHundred(LeapSolution leapSolution) =>
            EqualsExpression(
                LeapModuloExpression(leapSolution, 400),
                NumericLiteralExpression(0));

        private static BinaryExpressionSyntax LeapModuloExpression(LeapSolution leapSolution, int number) =>
            ModuloExpression(
                LeapParameterIdentifierName(leapSolution),
                NumericLiteralExpression(number));
    }
}