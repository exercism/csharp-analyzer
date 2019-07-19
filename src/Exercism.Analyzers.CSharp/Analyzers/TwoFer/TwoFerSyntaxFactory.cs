using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal static class TwoFerSyntaxFactory
    {
        public static ExpressionStatementSyntax TwoFerAssignParameterStatement(ExpressionSyntax condition, IdentifierNameSyntax parameterName) =>
            ExpressionStatement(
                SimpleAssignmentExpression(
                    parameterName,
                    condition));

        public static IfStatementSyntax TwoFerAssignParameterIfStatement(ExpressionSyntax condition, IdentifierNameSyntax parameterName) =>
            IfStatement(
                condition,
                Block(
                    SingletonList<StatementSyntax>(
                        ExpressionStatement(
                            SimpleAssignmentExpression(
                                parameterName,
                                StringLiteralExpression("you"))))));

        public static BinaryExpressionSyntax TwoFerParameterIsNullExpression(TwoFerSolution solution) =>
            EqualsExpression(
                TwoFerParameterIdentifierName(solution),
                NullLiteralExpression());

        public static IdentifierNameSyntax TwoFerParameterIdentifierName(TwoFerSolution solution) =>
            IdentifierName(solution.SpeakMethodParameterName);

        public static IdentifierNameSyntax TwoFerVariableIdentifierName(TwoFerSolution solution) =>
            IdentifierName(solution.TwoFerVariableName);

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

        public static ConditionalExpressionSyntax TwoFerConditionalExpressionWithNullCheck(TwoFerSolution solution) =>
            TwoFerConditionalExpression(
                EqualsExpression(
                    IdentifierName(solution.SpeakMethodParameterName),
                    NullLiteralExpression()),
                IdentifierName(solution.SpeakMethodParameterName));

        public static BinaryExpressionSyntax TwoFerStringConcatenationExpression(ExpressionSyntax nameExpression) =>
            AddExpression(
                AddExpression(
                    StringLiteralExpression("One for "),
                    nameExpression),
                StringLiteralExpression(", one for me."));

        public static InvocationExpressionSyntax TwoFerStringFormatInvocationExpression(ExpressionSyntax argumentExpression) =>
            InvocationExpression(
                StringMemberAccessExpression(
                    IdentifierName("Format")))
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

        private static InvocationExpressionSyntax TwoFerStringInvocationExpression(TwoFerSolution twoFerSolution, IdentifierNameSyntax stringMethodIdentifierName) =>
            StringInvocationExpression(
                stringMethodIdentifierName,
                TwoFerParameterIdentifierName(twoFerSolution));

        public static InterpolatedStringExpressionSyntax TwoFerConditionalInterpolatedStringExpression(ExpressionSyntax condition, IdentifierNameSyntax identifierName) =>
            TwoFerInterpolatedStringExpression(
                ParenthesizedExpression(
                    TwoFerConditionalExpression(condition, identifierName)));

        public static ConditionalExpressionSyntax TwoFerParameterIsNullConditionalExpression(TwoFerSolution solution) =>
            TwoFerConditionalExpression(
                TwoFerParameterIsNullExpression(solution),
                TwoFerParameterIdentifierName(solution));

        public static ConditionalExpressionSyntax TwoFerParameterIsNullOrEmptyConditionalExpression(TwoFerSolution solution) =>
            TwoFerConditionalExpression(
                TwoFerIsNullOrEmptyInvocationExpression(solution),
                TwoFerParameterIdentifierName(solution));

        public static ConditionalExpressionSyntax TwoFerParameterIsNullOrWhiteSpaceConditionalExpression(TwoFerSolution solution) =>
            TwoFerConditionalExpression(
                TwoFerIsNullOrWhiteSpaceInvocationExpression(solution),
                TwoFerParameterIdentifierName(solution));
    }
}