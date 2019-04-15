using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax
{
    public static class ReturnStatementSyntaxExtensions
    {
        public static bool ReturnsVariable(this ReturnStatementSyntax returnStatement, VariableDeclaratorSyntax variableDeclarator) =>
            returnStatement.Expression.IsEquivalentWhenNormalized(
                IdentifierName(variableDeclarator.Identifier));
        
        public static bool ReturnsParameter(this ReturnStatementSyntax returnStatement, ParameterSyntax parameter) =>
            returnStatement.Expression.IsEquivalentWhenNormalized(
                IdentifierName(parameter.Identifier));
    }
}