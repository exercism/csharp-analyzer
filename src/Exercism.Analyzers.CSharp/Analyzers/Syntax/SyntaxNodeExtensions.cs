using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax
{
    internal static class SyntaxNodeExtensions
    {
        public static SyntaxNode Simplify(this SyntaxNode reducedSyntaxRoot) =>
            SyntaxNodeSimplifier.Simplify(reducedSyntaxRoot);

        public static bool IsEquivalentWhenNormalized(this SyntaxNode node, SyntaxNode other) =>
            SyntaxNodeComparer.IsEquivalentToNormalized(node, other);

        public static IEnumerable<TSyntaxNode> DescendantNodes<TSyntaxNode>(this SyntaxNode node)
            where TSyntaxNode : SyntaxNode =>
            node.DescendantNodes().OfType<TSyntaxNode>();

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

        public static bool AssignsToIdentifier(this SyntaxNode syntaxNode, IdentifierNameSyntax identifierName) =>
            syntaxNode?
                .DescendantNodes<AssignmentExpressionSyntax>()
                .Any(assignmentExpression => assignmentExpression.Left.IsEquivalentWhenNormalized(identifierName)) ?? false;

        public static bool CreatesObjectOfType<T>(this SyntaxNode syntaxNode) =>
            syntaxNode?
                .DescendantNodes<ObjectCreationExpressionSyntax>()
                .Any(objectCreationExpression => objectCreationExpression.Type.IsEquivalentWhenNormalized(
                    IdentifierName(typeof(T).Name))) ?? false;

        public static bool ThrowsExceptionOfType<TException>(this SyntaxNode syntaxNode) where TException : Exception =>
            syntaxNode?
                .DescendantNodes<ThrowStatementSyntax>()
                .Any(throwsStatement =>
                        throwsStatement.Expression is ObjectCreationExpressionSyntax objectCreationExpression &&
                        objectCreationExpression.Type.IsEquivalentWhenNormalized(
                            IdentifierName(typeof(TException).Name))) ?? false;

        public static bool InvokesMethod(this SyntaxNode syntaxNode, MemberAccessExpressionSyntax memberAccessExpression) =>
            syntaxNode?
                .DescendantNodes<InvocationExpressionSyntax>()
                .Any(invocationExpression => invocationExpression.Expression.IsEquivalentWhenNormalized(memberAccessExpression)) ?? false;

        public static bool InvokesMethod(this SyntaxNode syntaxNode, SimpleNameSyntax methodName) =>
            syntaxNode?
                .DescendantNodes<InvocationExpressionSyntax>()
                .Any(invocationExpression =>
                        invocationExpression.Expression is MemberAccessExpressionSyntax memberAccessExpression &&
                        memberAccessExpression.Name.IsEquivalentWhenNormalized(methodName)) ?? false;
    }
}