using Exercism.Analyzers.CSharp.Analyzers.Shared;
using Exercism.Analyzers.CSharp.Analyzers.Syntax.Comparison;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax
{
    internal static class InvocationExpressionSyntaxExtensions
    {
        public static ExpressionSyntax FirstArgumentExpression(this InvocationExpressionSyntax invocationExpression) =>
            invocationExpression?.ArgumentList.Arguments[0].Expression;

        public static bool AssignedToVariable(this InvocationExpressionSyntax invocationExpression, out VariableDeclaratorSyntax variableDeclarator)
        {
            variableDeclarator =
                invocationExpression != null &&
                invocationExpression.Parent is EqualsValueClauseSyntax equalsValueClause
                    ? equalsValueClause.Parent as VariableDeclaratorSyntax
                    : null;

            return variableDeclarator != null;
        }

        public static bool AssignedToParameter(this InvocationExpressionSyntax invocationExpression, ParameterSyntax parameter) =>
            invocationExpression != null &&
            invocationExpression.Parent is AssignmentExpressionSyntax assignmentExpression
            && assignmentExpression.Left.IsEquivalentWhenNormalized(SharedSyntaxFactory.IdentifierName(parameter));
    }
}