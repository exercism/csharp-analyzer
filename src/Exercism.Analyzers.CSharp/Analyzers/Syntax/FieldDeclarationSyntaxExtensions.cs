using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax
{
    internal static class FieldDeclarationSyntaxExtensions
    {
        public static bool IsConst(this FieldDeclarationSyntax fieldDeclaration) =>
            fieldDeclaration.HasModifier(SyntaxKind.ConstKeyword);
        
        public static bool IsPrivate(this FieldDeclarationSyntax fieldDeclaration) =>
            fieldDeclaration.HasModifier(SyntaxKind.PrivateKeyword);

        private static bool HasModifier(this FieldDeclarationSyntax fieldDeclaration, SyntaxKind expected) =>
            fieldDeclaration?.Modifiers.Any(
                modifier => modifier.IsKind(expected)) ?? false;
    }
}