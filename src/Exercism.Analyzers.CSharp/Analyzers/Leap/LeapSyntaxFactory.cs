using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Leap
{
    internal static class LeapSyntaxFactory
    {
        public static MemberAccessExpressionSyntax IsLeapYearMemberAccessExpression() =>
            DateTimeMemberAccessExpression(IdentifierName("IsLeapYear"));

        public static IdentifierNameSyntax LeapParameterIdentifierName(LeapSolution solution) =>
            IdentifierName(solution.YearParameterName);

        public static BinaryExpressionSyntax LeapMinimumNumberOfChecksWithoutParenthesesBinaryExpression(LeapSolution solution) =>
            LogicalOrExpression(
                LogicalAndExpression(
                    DivisibleByFour(solution),
                    NotDivisibleByHundred(solution)),
                DivisibleByFourHundred(solution));

        public static BinaryExpressionSyntax LeapMinimumNumberOfChecksWithoutParenthesesBinaryExpressionReversed(LeapSolution solution) =>
            LogicalOrExpression(
                DivisibleByFourHundred(solution),
                LogicalAndExpression(
                    NotDivisibleByHundred(solution),
                    DivisibleByFour(solution)));

        public static BinaryExpressionSyntax LeapMinimumNumberOfChecksWithParenthesesBinaryExpression(LeapSolution solution) =>
            LogicalAndExpression(
                DivisibleByFour(solution),
                ParenthesizedExpression(
                    LogicalOrExpression(
                        NotDivisibleByHundred(solution),
                        DivisibleByFourHundred(solution))));

        private static BinaryExpressionSyntax DivisibleByFour(LeapSolution solution) =>
            EqualsExpression(
                LeapYearModuloExpression(solution, 4),
                NumericLiteralExpression(0));

        private static BinaryExpressionSyntax NotDivisibleByHundred(LeapSolution solution) =>
            NotEqualsExpression(
                LeapYearModuloExpression(solution, 100),
                NumericLiteralExpression(0));

        private static BinaryExpressionSyntax DivisibleByFourHundred(LeapSolution solution) =>
            EqualsExpression(
                LeapYearModuloExpression(solution, 400),
                NumericLiteralExpression(0));

        private static BinaryExpressionSyntax LeapYearModuloExpression(LeapSolution solution, int year) =>
            ModuloExpression(
                LeapParameterIdentifierName(solution),
                NumericLiteralExpression(year));
    }
}