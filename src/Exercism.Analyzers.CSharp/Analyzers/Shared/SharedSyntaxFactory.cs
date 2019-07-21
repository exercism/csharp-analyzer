using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Shared
{
    internal static class SharedSyntaxFactory
    {
        public static BinaryExpressionSyntax EqualsExpression(ExpressionSyntax left, ExpressionSyntax right) =>
            BinaryExpression(SyntaxKind.EqualsExpression, left, right);

        public static BinaryExpressionSyntax NotEqualsExpression(ExpressionSyntax left, ExpressionSyntax right) =>
            BinaryExpression(SyntaxKind.NotEqualsExpression, left, right);

        public static BinaryExpressionSyntax AddExpression(ExpressionSyntax left, ExpressionSyntax right) =>
            BinaryExpression(SyntaxKind.AddExpression, left, right);

        public static BinaryExpressionSyntax LogicalOrExpression(ExpressionSyntax left, ExpressionSyntax right) =>
            BinaryExpression(SyntaxKind.LogicalOrExpression, left, right);

        public static BinaryExpressionSyntax LogicalAndExpression(ExpressionSyntax left, ExpressionSyntax right) =>
            BinaryExpression(SyntaxKind.LogicalAndExpression, left, right);

        public static BinaryExpressionSyntax ModuloExpression(ExpressionSyntax left, ExpressionSyntax right) =>
            BinaryExpression(SyntaxKind.ModuloExpression, left, right);

        public static BinaryExpressionSyntax CoalesceExpression(ExpressionSyntax left, ExpressionSyntax right) =>
            BinaryExpression(SyntaxKind.CoalesceExpression, left, right);

        public static LiteralExpressionSyntax NullLiteralExpression() =>
            LiteralExpression(SyntaxKind.NullLiteralExpression);

        public static LiteralExpressionSyntax NumericLiteralExpression(int value) =>
            LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value));

        public static LiteralExpressionSyntax NumericLiteralExpression(string text, double value) =>
            LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(text, value));

        public static LiteralExpressionSyntax StringLiteralExpression(string value) =>
            LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(value));

        public static InvocationExpressionSyntax StringInvocationExpression(IdentifierNameSyntax methodName, IdentifierNameSyntax argumentName) =>
            InvocationExpression(
                StringMemberAccessExpression(methodName),
                Argument(argumentName));

        public static MemberAccessExpressionSyntax StringMemberAccessExpression(IdentifierNameSyntax name) =>
            SimpleMemberAccessExpression(PredefinedType(Token(SyntaxKind.StringKeyword)), name);

        public static MemberAccessExpressionSyntax DateTimeMemberAccessExpression(IdentifierNameSyntax name) =>
            SimpleMemberAccessExpression(SyntaxFactory.IdentifierName("DateTime"), name);

        public static MemberAccessExpressionSyntax SimpleMemberAccessExpression(ExpressionSyntax expression, IdentifierNameSyntax name) =>
            MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                expression,
                name);

        public static InvocationExpressionSyntax InvocationExpression(ExpressionSyntax expression, params ArgumentSyntax[] arguments) =>
            SyntaxFactory.InvocationExpression(expression)
                .WithArgumentList(
                    ArgumentList(
                        SeparatedArgumentList(arguments)));

        private static SeparatedSyntaxList<ArgumentSyntax> SeparatedArgumentList(ArgumentSyntax[] arguments)
        {
            var syntaxNodesOrTokens = new List<SyntaxNodeOrToken>(capacity: arguments.Length * 2);

            for (var i = 0; i < arguments.Length; i++)
            {
                if (i > 0)
                    syntaxNodesOrTokens.Add(Token(SyntaxKind.CommaToken));

                syntaxNodesOrTokens.Add(arguments[i]);
            }

            return SeparatedList<ArgumentSyntax>(syntaxNodesOrTokens);
        }

        public static AssignmentExpressionSyntax SimpleAssignmentExpression(ExpressionSyntax left, ExpressionSyntax right) =>
            AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, left, right);

        public static InterpolatedStringTextSyntax InterpolatedStringText(string text) =>
            SyntaxFactory.InterpolatedStringText(
                Token(
                    TriviaList(),
                    SyntaxKind.InterpolatedStringTextToken,
                    text,
                    text,
                    TriviaList()));

        public static IdentifierNameSyntax IdentifierName(ParameterSyntax parameter) =>
            SyntaxFactory.IdentifierName(parameter.Identifier);

        public static IdentifierNameSyntax IdentifierName(VariableDeclaratorSyntax variableDeclarator) =>
            SyntaxFactory.IdentifierName(variableDeclarator.Identifier);
    }
}