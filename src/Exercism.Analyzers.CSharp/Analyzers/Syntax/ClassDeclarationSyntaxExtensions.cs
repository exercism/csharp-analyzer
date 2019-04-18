using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax
{
    internal static class ClassDeclarationSyntaxExtensions
    {
        public static VariableDeclaratorSyntax AssignedVariableWithName(this ClassDeclarationSyntax classDeclaration, IdentifierNameSyntax identifierName)
        {
            if (classDeclaration == null || identifierName == null)
                return null;
            
            return classDeclaration
                .DescendantNodes<VariableDeclaratorSyntax>()
                .FirstOrDefault(variableDeclarator => variableDeclarator.Identifier.IsEquivalentWhenNormalized(identifierName.Identifier));
        }
    }
}