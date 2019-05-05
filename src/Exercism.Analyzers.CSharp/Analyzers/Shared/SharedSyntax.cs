using System;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Exercism.Analyzers.CSharp.Analyzers.Syntax.Comparison;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Shared
{
    internal static class SharedSyntax
    {
        public static ArgumentType ArgumentDefinedAs(FieldDeclarationSyntax fieldDeclaration, LocalDeclarationStatementSyntax localDeclarationStatement, ExpressionSyntax expression)
        {
            if (fieldDeclaration != null)
                return ArgumentType.Field;
            
            if (localDeclarationStatement != null)
                return ArgumentType.Local;

            if (expression != null)
                return ArgumentType.Expression;

            return ArgumentType.Unknown;
        }

        public static ReturnType ReturnedAs(InvocationExpressionSyntax invocationExpression, ExpressionSyntax returnedExpression, ParameterSyntax parameter)
        {
            if (invocationExpression.AssignedToVariable(out var variableDeclarator))
                return returnedExpression.IsEquivalentWhenNormalized(SharedSyntaxFactory.IdentifierName(variableDeclarator))
                    ? ReturnType.VariableAssignment
                    : ReturnType.Unknown;

            if (invocationExpression.AssignedToParameter(parameter))
                return returnedExpression.IsEquivalentWhenNormalized(SharedSyntaxFactory.IdentifierName(parameter))
                    ? ReturnType.ParameterAssigment
                    : ReturnType.Unknown;

            return returnedExpression.IsEquivalentWhenNormalized(invocationExpression)
                ? ReturnType.ImmediateValue
                : ReturnType.Unknown;
        }

        public static ExpressionSyntax ArgumentValueExpression(ArgumentType argumentType, ExpressionSyntax argumentExpression, VariableDeclaratorSyntax addSecondsArgumentVariable)
        {
            switch (argumentType)
            {
                case ArgumentType.Unknown:
                    return null;
                case ArgumentType.Expression:
                    return argumentExpression;
                case ArgumentType.Local:
                case ArgumentType.Field:
                    return addSecondsArgumentVariable.Initializer.Value;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}