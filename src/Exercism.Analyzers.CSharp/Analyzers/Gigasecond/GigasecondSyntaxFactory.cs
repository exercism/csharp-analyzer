using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal static class GigasecondSyntaxFactory
    {
        public static InvocationExpressionSyntax GigasecondAddSecondsWithDigitsWithoutSeparatorInvocationExpression(GigasecondSolution gigasecondSolution) =>
            GigasecondAddSecondsInvocationExpression(
                gigasecondSolution,
                NumericLiteralExpression(1000000000));

        public static InvocationExpressionSyntax GigasecondAddSecondsWithDigitsWithSeparatorInvocationExpression(GigasecondSolution gigasecondSolution) =>
            GigasecondAddSecondsInvocationExpression(
                gigasecondSolution,
                NumericLiteralExpression("1_000_000_000", 1000000000));
        
        public static InvocationExpressionSyntax GigasecondAddSecondsWithScientificNotationInvocationExpression(GigasecondSolution gigasecondSolution) =>
            GigasecondAddSecondsInvocationExpression(
                gigasecondSolution,
                NumericLiteralExpression("1E9", 1e9));

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

        public static IdentifierNameSyntax GigasecondParameterIdentifierName(GigasecondSolution gigasecondSolution) =>
            IdentifierName(gigasecondSolution.BirthDateParameter);

        private static ArgumentSyntax GigasecondBirthDateArgument(ExpressionSyntax argumentExpression) =>
            Argument(argumentExpression);
        
        public static InvocationExpressionSyntax GigasecondAddSecondsWithMathPowInvocationExpression() =>
            InvocationExpression(
                GigasecondMathPowExpression(),
                Argument(
                    NumericLiteralExpression(10)),
                Argument(
                    NumericLiteralExpression(9)));

        private static MemberAccessExpressionSyntax GigasecondMathPowExpression() =>
            SimpleMemberAccessExpression(
                IdentifierName("Math"),
                IdentifierName("Pow"));
    }
}