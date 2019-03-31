using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal static class TwoFerSyntaxFactory
    {
        public static BlockSyntax TwoFerAssignParameterToYou(TwoFerSolution twoFerSolution) =>
            Block(
                SingletonList<StatementSyntax>(
                    ExpressionStatement(
                        SimpleAssignmentExpression(
                            TwoFerParameterIdentifierName(twoFerSolution),
                            StringLiteralExpression("you")))));

        public static BinaryExpressionSyntax TwoFerParameterIsNullExpression(TwoFerSolution twoFerSolution) =>
            EqualsExpression(
                TwoFerParameterIdentifierName(twoFerSolution),
                NullLiteralExpression());

        public static IdentifierNameSyntax TwoFerParameterIdentifierName(TwoFerSolution twoFerSolution) =>
            IdentifierName(twoFerSolution.InputParameter.Identifier);

        public static BinaryExpressionSyntax TwoFerCoalesceExpression(IdentifierNameSyntax identifierName) =>
            CoalesceExpression(
                identifierName,
                StringLiteralExpression("you"));

        public static InterpolatedStringExpressionSyntax TwoFerInterpolatedStringExpression(ExpressionSyntax interpolationExpression) =>
            InterpolatedStringExpression(
                    Token(SyntaxKind.InterpolatedStringStartToken))
                .WithContents(
                    List(new InterpolatedStringContentSyntax[]
                        {
                            InterpolatedStringText("One for "),
                            Interpolation(interpolationExpression),
                            InterpolatedStringText(", one for me.")
                        }));

        public static ConditionalExpressionSyntax TwoFerConditionalExpression(ExpressionSyntax condition, IdentifierNameSyntax identifierName) =>
            ConditionalExpression(
                condition,
                StringLiteralExpression("you"),
                identifierName);

        public static ConditionalExpressionSyntax TwoFerTernaryOperatorConditionalExpression(ParameterSyntax inputParameter) =>
            TwoFerConditionalExpression(
                EqualsExpression(
                    IdentifierName(inputParameter.Identifier),
                    NullLiteralExpression()),
                IdentifierName(inputParameter.Identifier));

        public static BinaryExpressionSyntax TwoFerStringConcatenationExpression(ExpressionSyntax middleExpression) =>
            AddExpression(
                AddExpression(
                    StringLiteralExpression("One for "),
                    middleExpression),
                StringLiteralExpression(", one for me."));

        public static InvocationExpressionSyntax TwoFerStringFormatInvocationExpression(ExpressionSyntax argumentExpression) =>
            InvocationExpression(
                StringMemberAccessExpression(IdentifierName("Format")))
                .WithArgumentList(
                    ArgumentList(
                        SeparatedList<ArgumentSyntax>(
                            new SyntaxNodeOrToken[]{
                                Argument(
                                    StringLiteralExpression("One for {0}, one for me.")),
                                Token(SyntaxKind.CommaToken),
                                Argument(argumentExpression)})));

        public static InvocationExpressionSyntax TwoFerIsNullOrEmptyInvocationExpression(TwoFerSolution twoFerSolution) =>
            TwoFerStringInvocationExpression(twoFerSolution, IdentifierName("IsNullOrEmpty"));

        public static InvocationExpressionSyntax TwoFerIsNullOrWhiteSpaceInvocationExpression(TwoFerSolution twoFerSolution) =>
            TwoFerStringInvocationExpression(twoFerSolution, IdentifierName("IsNullOrWhiteSpace"));

        public static InvocationExpressionSyntax TwoFerStringInvocationExpression(TwoFerSolution twoFerSolution, IdentifierNameSyntax stringMethodIdentifierName) =>
            StringInvocationExpression(stringMethodIdentifierName, TwoFerParameterIdentifierName(twoFerSolution));
    }
}