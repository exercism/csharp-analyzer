using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class SyntaxNodeExtensions
    {
        public static ClassDeclarationSyntax GetClass(this SyntaxNode syntaxNode, string className) =>
            syntaxNode?
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .SingleOrDefault(syntax => syntax.Identifier.Text == className);

        public static MethodDeclarationSyntax GetMethod(this SyntaxNode syntaxNode, string methodName) =>
            syntaxNode?
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .SingleOrDefault(syntax => syntax.Identifier.Text == methodName);

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

        public static bool HasParameter(this MethodDeclarationSyntax methodDeclaration, string parameterName) =>
            methodDeclaration?
                .ParameterList
                .Parameters
                .Any(parameter => parameter.Identifier.ValueText == parameterName) ?? false;

        public static bool AssignsToParameter(this MethodDeclarationSyntax methodDeclaration, string parameterName) =>
            methodDeclaration.HasParameter(parameterName) &&
            methodDeclaration.AssignsToIdentifier(parameterName);

        public static bool AssignsToIdentifier(this SyntaxNode syntaxNode, string identifierName) =>
            syntaxNode?
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .Any(assignmentExpression => assignmentExpression.Left is IdentifierNameSyntax name &&
                                             name.Identifier.ValueText == identifierName) ?? false;
    }
}