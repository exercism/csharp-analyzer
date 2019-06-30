using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Syntax.Comparison;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax
{
    internal static class ClassDeclarationSyntaxExtensions
    {
        public static VariableDeclaratorSyntax ArgumentVariable(this ClassDeclarationSyntax classDeclaration, ExpressionSyntax argumentExpression) =>
            classDeclaration.AssignedVariableWithName(argumentExpression as IdentifierNameSyntax);

        public static VariableDeclaratorSyntax AssignedVariableWithName(this ClassDeclarationSyntax classDeclaration, IdentifierNameSyntax variableIdentifierName)
        {
            if (classDeclaration == null || variableIdentifierName == null)
                return null;
            
            return classDeclaration
                .DescendantNodes<VariableDeclaratorSyntax>()
                .FirstOrDefault(variableDeclarator => variableDeclarator.Identifier.IsEquivalentWhenNormalized(variableIdentifierName.Identifier));
        }
    }
}