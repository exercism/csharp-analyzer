using System;
using System.Collections.Generic;
using System.Linq;

using Exercism.Analyzers.CSharp.Syntax.Comparison;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Syntax
{
    internal static class SyntaxNodeExtensions
    {
        public static IEnumerable<TSyntaxNode> DescendantNodes<TSyntaxNode>(this SyntaxNode syntaxNode)
            where TSyntaxNode : SyntaxNode =>
            syntaxNode?.DescendantNodes().OfType<TSyntaxNode>() ?? Enumerable.Empty<TSyntaxNode>();

        public static MethodDeclarationSyntax GetClassMethod(this SyntaxNode syntaxNode, string className,
            string methodName) => syntaxNode.GetClass(className).GetMethod(methodName);

        public static ClassDeclarationSyntax GetClass(this SyntaxNode syntaxNode, string className) =>
            syntaxNode?
                .DescendantNodes<ClassDeclarationSyntax>()
                .FirstOrDefault(syntax => syntax.Identifier.Text == className);

        public static MethodDeclarationSyntax GetMethod(this SyntaxNode syntaxNode, string methodName) =>
            syntaxNode?
                .DescendantNodes<MethodDeclarationSyntax>()
                .FirstOrDefault(syntax => syntax.Identifier.Text == methodName);

        public static IEnumerable<MethodDeclarationSyntax> GetMethods(this SyntaxNode syntaxNode, string methodName) =>
            syntaxNode?
                .DescendantNodes<MethodDeclarationSyntax>()
                .Where(syntax => syntax.Identifier.Text == methodName) ?? Enumerable.Empty<MethodDeclarationSyntax>();

        public static PropertyDeclarationSyntax GetProperty(this SyntaxNode syntaxNode, string propertyName) =>
            syntaxNode?
                .DescendantNodes<PropertyDeclarationSyntax>()
                .FirstOrDefault(syntax => syntax.Identifier.Text == propertyName);

        public static IEnumerable<PropertyDeclarationSyntax> GetProperties(this SyntaxNode syntaxNode, string propertyName) =>
            syntaxNode?
                .DescendantNodes<PropertyDeclarationSyntax>()
                .Where(syntax => syntax.Identifier.Text == propertyName) ?? Enumerable.Empty<PropertyDeclarationSyntax>();

        public static FieldDeclarationSyntax GetField(this SyntaxNode syntaxNode, string variable) =>
            syntaxNode?
                .DescendantNodes<VariableDeclaratorSyntax>()
                .FirstOrDefault(syntax => IsEqualFieldName(syntax.Identifier.Text, variable))
                ?.FieldDeclaration();

        public static IEnumerable<FieldDeclarationSyntax> GetFields(this SyntaxNode syntaxNode, string variable) =>
            syntaxNode?
                .DescendantNodes<VariableDeclaratorSyntax>()
                .Where(syntax => IsEqualFieldName(syntax.Identifier.Text, variable))
                .Select(v => v?.FieldDeclaration()) ?? Enumerable.Empty<FieldDeclarationSyntax>();

        public static bool IsEqualFieldName(string name, string expectedName)
        {
            var expected = new[] { expectedName, expectedName.StartsWith("_") ? expectedName.Substring(1) : "_" + expectedName };
            return expected.Contains(name);
        }

        public static bool AssignsToIdentifier(this SyntaxNode syntaxNode, IdentifierNameSyntax identifierName) =>
            syntaxNode?
                .DescendantNodes<AssignmentExpressionSyntax>()
                .Any(assignmentExpression => assignmentExpression.Left.IsEquivalentWhenNormalized(identifierName)) ?? false;

        public static bool CreatesObjectOfType<T>(this SyntaxNode syntaxNode) =>
            syntaxNode?
                .DescendantNodes<ObjectCreationExpressionSyntax>()
                .Any(objectCreationExpression => objectCreationExpression.Type.IsEquivalentWhenNormalized(
                    SyntaxFactory.IdentifierName(typeof(T).Name))) ?? false;

        public static bool ThrowsExceptionOfType<TException>(this SyntaxNode syntaxNode) where TException : Exception =>
            syntaxNode?
                .DescendantNodes<ThrowStatementSyntax>()
                .Any(throwsStatement =>
                        throwsStatement.Expression is ObjectCreationExpressionSyntax objectCreationExpression &&
                        objectCreationExpression.Type.IsEquivalentWhenNormalized(
                            SyntaxFactory.IdentifierName(typeof(TException).Name))) ?? false;

        public static bool InvokesMethod(this SyntaxNode syntaxNode, MemberAccessExpressionSyntax memberAccessExpression) =>
            syntaxNode?
                .DescendantNodes<InvocationExpressionSyntax>()
                .Any(invocationExpression => invocationExpression.Expression.IsEquivalentWhenNormalized(memberAccessExpression)) ?? false;

        public static bool InvokesMethod(this SyntaxNode syntaxNode, string identifier) =>
            syntaxNode?
                .DescendantNodes<InvocationExpressionSyntax>()
                .Any(invocationExpression => invocationExpression.InvokesMethod(identifier)) ?? false;

        private static bool InvokesMethod(this InvocationExpressionSyntax invocationExpression, string identifier) =>
            invocationExpression?.Expression.WithoutTrivia().ToFullString() == identifier;

        public static bool InvokesMethod(this SyntaxNode syntaxNode, SimpleNameSyntax methodName) =>
            syntaxNode?
                .DescendantNodes<InvocationExpressionSyntax>()
                .Any(invocationExpression =>
                    invocationExpression.Expression is MemberAccessExpressionSyntax memberAccessExpression &&
                    memberAccessExpression.Name.IsEquivalentWhenNormalized(methodName)) ?? false;

        public static InvocationExpressionSyntax InvocationExpression(this SyntaxNode syntaxNode, ExpressionSyntax expression) =>
            syntaxNode?
                .DescendantNodes<InvocationExpressionSyntax>()
                .FirstOrDefault(invocationExpression => invocationExpression.Expression.IsEquivalentWhenNormalized(expression));
    }
}