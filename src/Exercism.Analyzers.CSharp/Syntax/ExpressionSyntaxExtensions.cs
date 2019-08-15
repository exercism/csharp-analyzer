using Exercism.Analyzers.CSharp.Analyzers.Shared;
using Exercism.Analyzers.CSharp.Syntax.Comparison;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Syntax
{
    internal static class ExpressionSyntaxExtensions
    {
        public static bool AssignedToVariable(this ExpressionSyntax expression, out VariableDeclaratorSyntax variableDeclarator)
        {
            variableDeclarator =
                expression != null &&
                expression.Parent is EqualsValueClauseSyntax equalsValueClause
                    ? equalsValueClause.Parent as VariableDeclaratorSyntax
                    : null;

            return variableDeclarator != null;
        }

        public static bool AssignedToParameter(this ExpressionSyntax expression, ParameterSyntax parameter) =>
            expression != null &&
            expression.Parent is AssignmentExpressionSyntax assignmentExpression
            && assignmentExpression.Left.IsEquivalentWhenNormalized(SharedSyntaxFactory.IdentifierName(parameter));
    }
}