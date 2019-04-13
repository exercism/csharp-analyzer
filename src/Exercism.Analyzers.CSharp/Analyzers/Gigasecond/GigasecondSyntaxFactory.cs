using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
        
        public static MemberAccessExpressionSyntax GigasecondAddMemberAccessExpression(GigasecondSolution gigasecondSolution) =>
            GigasecondMemberAccessExpression(gigasecondSolution, IdentifierName("Add"));
        
        public static InvocationExpressionSyntax GigasecondAddSecondsInvocationExpression(GigasecondSolution gigasecondSolution, ExpressionSyntax argumentExpression) =>
            // TODO: consider adding factory method to invocation with argument
            InvocationExpression(
                    GigasecondMemberAccessExpression(gigasecondSolution, IdentifierName("AddSeconds")))
                .WithArgumentList(
                    ArgumentList(
                        SingletonSeparatedList(
                            GigasecondBirthDateArgument(argumentExpression))));

        private static MemberAccessExpressionSyntax GigasecondMemberAccessExpression(GigasecondSolution gigasecondSolution, IdentifierNameSyntax methodName) =>
            SimpleMemberAccessExpression(
                GigasecondParameterIdentifierName(gigasecondSolution),
                methodName);

        public static IdentifierNameSyntax GigasecondParameterIdentifierName(GigasecondSolution gigasecondSolution) =>
            IdentifierName(gigasecondSolution.BirthDateParameter.Identifier);

        private static ArgumentSyntax GigasecondBirthDateArgument(ExpressionSyntax argumentExpression) =>
            Argument(argumentExpression);
        
        public static InvocationExpressionSyntax GigasecondAddSecondsWithMathPowInvocationExpression() =>
            // TODO: consider adding factory method to invocation with arguments
            InvocationExpression(
                    GigasecondMathPowExpression())
                .WithArgumentList(
                    ArgumentList(
                        SeparatedList<ArgumentSyntax>(
                            new SyntaxNodeOrToken[]{
                                Argument(
                                    NumericLiteralExpression(10)),
                                Token(SyntaxKind.CommaToken),
                                Argument(
                                    NumericLiteralExpression(9))})));

        private static MemberAccessExpressionSyntax GigasecondMathPowExpression() =>
            SimpleMemberAccessExpression(
                IdentifierName("Math"),
                IdentifierName("Pow"));
    }
}