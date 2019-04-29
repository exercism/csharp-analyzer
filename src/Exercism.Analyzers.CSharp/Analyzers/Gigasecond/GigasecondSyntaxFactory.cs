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
        
        public static InvocationExpressionSyntax GigasecondAddSecondsWithDigitsWithoutSeparatorInvocationExpression(GigasecondSolution gigasecondSolution) =>
            GigasecondAddSecondsInvocationExpression(
                gigasecondSolution,
                GigasecondAsDigitsWithoutSeparator());

        public static InvocationExpressionSyntax GigasecondAddSecondsWithDigitsWithSeparatorInvocationExpression(GigasecondSolution gigasecondSolution) =>
            GigasecondAddSecondsInvocationExpression(
                gigasecondSolution,
                GigasecondAsDigitsWithSeparator());

        public static InvocationExpressionSyntax GigasecondAddSecondsWithScientificNotationInvocationExpression(GigasecondSolution gigasecondSolution) =>
            GigasecondAddSecondsInvocationExpression(
                gigasecondSolution,
                GigasecondAsScientificNotation());

        public static InvocationExpressionSyntax GigasecondAddSecondsInvocationExpression(GigasecondSolution gigasecondSolution, ExpressionSyntax argumentExpression) =>
            InvocationExpression(
                GigasecondAddSecondsMemberAccessExpression(gigasecondSolution),
                GigasecondBirthDateArgument(argumentExpression));

        public static MemberAccessExpressionSyntax GigasecondAddSecondsMemberAccessExpression(GigasecondSolution gigasecondSolution) =>
            GigasecondParameterMemberAccessExpression(
                gigasecondSolution,
                IdentifierName("AddSeconds"));

        private static MemberAccessExpressionSyntax GigasecondParameterMemberAccessExpression(GigasecondSolution gigasecondSolution, IdentifierNameSyntax methodName) =>
            SimpleMemberAccessExpression(
                GigasecondParameterIdentifierName(gigasecondSolution),
                methodName);

        private static IdentifierNameSyntax GigasecondParameterIdentifierName(GigasecondSolution gigasecondSolution) =>
            IdentifierName(gigasecondSolution.AddMethodParameter);

        private static ArgumentSyntax GigasecondBirthDateArgument(ExpressionSyntax argumentExpression) =>
            Argument(argumentExpression);
        
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
    }
}