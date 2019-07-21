using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal static class GigasecondSyntaxFactory
    {
        public static LiteralExpressionSyntax GigasecondAsDigitsWithoutSeparator() =>
            NumericLiteralExpression(1000000000);

        public static LiteralExpressionSyntax GigasecondAsDigitsWithSeparator() =>
            NumericLiteralExpression("1_000_000_000", 1000000000);

        public static LiteralExpressionSyntax GigasecondAsScientificNotation() =>
            NumericLiteralExpression("1E9", 1e9);

        public static InvocationExpressionSyntax GigasecondAsMathPowInvocationExpression() =>
            InvocationExpression(
                GigasecondMathPowMemberAccessExpression(),
                Argument(
                    NumericLiteralExpression(10)),
                Argument(
                    NumericLiteralExpression(9)));

        private static MemberAccessExpressionSyntax GigasecondMathPowMemberAccessExpression() =>
            SimpleMemberAccessExpression(
                IdentifierName("Math"),
                IdentifierName("Pow"));

        public static MemberAccessExpressionSyntax AddSecondsMemberAccessExpression(GigasecondSolution solution) =>
            SimpleMemberAccessExpression(
                IdentifierName(solution.AddMethodParameterName),
                IdentifierName("AddSeconds"));
    }
}