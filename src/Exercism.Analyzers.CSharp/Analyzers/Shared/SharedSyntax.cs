using System;
using Exercism.Analyzers.CSharp.Syntax;
using Exercism.Analyzers.CSharp.Syntax.Comparison;
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

        public static ReturnType ReturnedAs(ExpressionSyntax expression, ExpressionSyntax returnedExpression, ParameterSyntax parameter)
        {
            if (expression.AssignedToVariable(out var variableDeclarator))
                return returnedExpression.IsEquivalentWhenNormalized(SharedSyntaxFactory.IdentifierName(variableDeclarator))
                    ? ReturnType.VariableAssignment
                    : ReturnType.Unknown;

            if (expression.AssignedToParameter(parameter))
                return returnedExpression.IsEquivalentWhenNormalized(SharedSyntaxFactory.IdentifierName(parameter))
                    ? ReturnType.ParameterAssigment
                    : ReturnType.Unknown;

            return returnedExpression.IsEquivalentWhenNormalized(expression)
                ? ReturnType.ImmediateValue
                : ReturnType.Unknown;
        }

        public static ExpressionSyntax ArgumentValueExpression(ArgumentType argumentType, ExpressionSyntax argumentExpression, VariableDeclaratorSyntax variableDeclarator)
        {
            switch (argumentType)
            {
                case ArgumentType.Unknown:
                    return null;
                case ArgumentType.Expression:
                    return argumentExpression;
                case ArgumentType.Local:
                case ArgumentType.Field:
                    return variableDeclarator.Initializer.Value;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}