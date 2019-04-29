using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Shared;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax
{
    public static class ArrowExpressionClauseSyntaxExtensions
    {
        public static bool UsesVariableAsArgument(this ArrowExpressionClauseSyntax arrowExpressionClause, VariableDeclaratorSyntax variableDeclarator) =>
            arrowExpressionClause.Expression is InvocationExpressionSyntax invocationExpression &&
            invocationExpression.ArgumentList.Arguments.Any(
                argument => argument.Expression.IsEquivalentWhenNormalized(SharedSyntaxFactory.IdentifierName(variableDeclarator)));

    }
}