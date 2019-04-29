using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax
{
    internal static class VariableDeclaratorSyntaxExtensions
    {
        public static LocalDeclarationStatementSyntax LocalDeclarationStatement(this VariableDeclaratorSyntax variableDeclarator) =>
            variableDeclarator != null &&
            variableDeclarator.Parent is VariableDeclarationSyntax variableDeclaration &&
            variableDeclaration.Parent is LocalDeclarationStatementSyntax localDeclarationStatement
                ? localDeclarationStatement
                : null;

        public static FieldDeclarationSyntax FieldDeclaration(this VariableDeclaratorSyntax variableDeclarator) =>
            variableDeclarator != null &&
            variableDeclarator.Parent is VariableDeclarationSyntax variableDeclaration &&
            variableDeclaration.Parent is FieldDeclarationSyntax fieldDeclaration
                ? fieldDeclaration
                : null;
    }
}