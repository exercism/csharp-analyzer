using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Shared
{
    internal static class SharedSyntaxFactory
    {
        public static BinaryExpressionSyntax EqualsExpression(IdentifierNameSyntax left, LiteralExpressionSyntax right) =>
            BinaryExpression(SyntaxKind.EqualsExpression, left, right);

        public static BinaryExpressionSyntax AddExpression(ExpressionSyntax left, ExpressionSyntax right) =>
            BinaryExpression(SyntaxKind.AddExpression, left, right);

        public static BinaryExpressionSyntax CoalesceExpression(IdentifierNameSyntax left, LiteralExpressionSyntax right) =>
            BinaryExpression(SyntaxKind.CoalesceExpression, left, right);
        
        public static LiteralExpressionSyntax NullLiteralExpression() =>
            LiteralExpression(SyntaxKind.NullLiteralExpression);
        
        public static LiteralExpressionSyntax StringLiteralExpression(string value) =>
            LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(value));
        
        public static InvocationExpressionSyntax StringInvocationExpression(IdentifierNameSyntax methodIdentifierName, IdentifierNameSyntax argumentIdentifierName) =>
            InvocationExpression(
                    StringMemberAccessExpression(methodIdentifierName))
                .WithArgumentList(
                    ArgumentList(
                        SingletonSeparatedList(
                            Argument(
                                argumentIdentifierName))));

        public static MemberAccessExpressionSyntax StringMemberAccessExpression(IdentifierNameSyntax methodIdentifierName) =>
            MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                PredefinedType(Token(SyntaxKind.StringKeyword)),
                methodIdentifierName);
        
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
    }
}