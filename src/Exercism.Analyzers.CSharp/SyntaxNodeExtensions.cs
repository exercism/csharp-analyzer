using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp
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
    }
}