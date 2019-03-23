using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax
{
    internal static class SyntaxNodeExtensions
    {
        public static ClassDeclarationSyntax GetClass(this SyntaxNode syntaxNode, string className) =>
            syntaxNode?
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(syntax => syntax.Identifier.Text == className);

        public static MethodDeclarationSyntax GetMethod(this SyntaxNode syntaxNode, string methodName) =>
            syntaxNode?
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(syntax => syntax.Identifier.Text == methodName);

        public static IEnumerable<MethodDeclarationSyntax> GetMethods(this SyntaxNode syntaxNode, string methodName) =>
            syntaxNode?
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Where(syntax => syntax.Identifier.Text == methodName) ?? Enumerable.Empty<MethodDeclarationSyntax>();

        public static bool HasClass(this SyntaxNode syntaxNode, string className) =>
            syntaxNode?
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Any(syntax => syntax.Identifier.Text == className) ?? false;

        public static bool HasMethod(this SyntaxNode syntaxNode, string methodName) =>
            syntaxNode?
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Any(syntax => syntax.Identifier.Text == methodName) ?? false;

        public static bool AssignsToIdentifier(this SyntaxNode syntaxNode, string identifierName) =>
            syntaxNode?
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .Any(assignmentExpression => assignmentExpression.Left is IdentifierNameSyntax name &&
                                             name.Identifier.ValueText == identifierName) ?? false;

        public static bool ThrowsException(this SyntaxNode syntaxNode, string exceptionNamespace, string exceptionType) =>
            syntaxNode?
                .DescendantNodes()
                .OfType<ThrowStatementSyntax>()
                .Any(throwsStatement =>
                    throwsStatement.Expression.CreatesObjectOfType(exceptionType.StripNamespace(exceptionNamespace)) ||
                    throwsStatement.Expression.CreatesObjectOfType($"{exceptionNamespace}.{exceptionType}")) ?? false;

        public static bool InvokesMethod(this SyntaxNode syntaxNode, string classNamespace, string className, string methodName) =>
            syntaxNode?
                .DescendantNodes()
                .OfType<MemberAccessExpressionSyntax>()
                .Any(memberAccessExpression => 
                    memberAccessExpression.ToFullString() == $"{classNamespace}.{className}.{methodName}" ||
                    memberAccessExpression.ToFullString() == $"{className.StripNamespace(classNamespace)}.{methodName}") ?? false;

        private static string StripNamespace(this string className, string classNamespace) => className.Replace($"{classNamespace}.", "");
    }
}