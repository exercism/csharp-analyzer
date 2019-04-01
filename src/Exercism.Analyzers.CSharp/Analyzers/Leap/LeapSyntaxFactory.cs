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
                EqualsExpression(
                    LeapModuloExpression(leapSolution, 4),
                    NumericLiteralExpression(0)),
                NotEqualsExpression(
                    LeapModuloExpression(leapSolution, 100),
                    NumericLiteralExpression(0))),
            EqualsExpression(
                LeapModuloExpression(leapSolution, 400),
                NumericLiteralExpression(0)));

        public static BinaryExpressionSyntax LeapMinimumNumberOfChecksWithParenthesesBinaryExpression(LeapSolution leapSolution) =>
                LogicalAndExpression(
                EqualsExpression(
                    LeapModuloExpression(leapSolution, 4),
                    NumericLiteralExpression(0)),
                ParenthesizedExpression(
                        LogicalOrExpression(
                        NotEqualsExpression(
                            LeapModuloExpression(leapSolution, 100),
                            NumericLiteralExpression(0)),
                    EqualsExpression(
                        LeapModuloExpression(leapSolution, 400),
                        NumericLiteralExpression(0)))));

        private static BinaryExpressionSyntax LeapModuloExpression(LeapSolution leapSolution, int number) =>
            ModuloExpression(
                LeapParameterIdentifierName(leapSolution),
                NumericLiteralExpression(number));
    }
}