using System;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Gigasecond.GigasecondSyntaxFactory;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal static class GigasecondSyntax
    {
        public static bool UsesExpressionBody(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddMethod.IsExpressionBody();

        public static bool UsesAddSecondsWithScientificNotation(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.Returns(
                GigasecondAddSecondsWithScientificNotationInvocationExpression(gigasecondSolution));

        public static bool UsesAddSecondsWithDigitsWithoutSeparator(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.Returns(
                GigasecondAddSecondsWithDigitsWithoutSeparatorInvocationExpression(gigasecondSolution));

        public static bool UsesAddSecondsWithDigitsWithSeparator(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.Returns(
                GigasecondAddSecondsWithDigitsWithSeparatorInvocationExpression(gigasecondSolution));

        public static bool UsesAddSecondsWithMathPow(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.Returns(
                GigasecondAddSecondsInvocationExpression(
                    gigasecondSolution,
                    GigasecondAddSecondsWithMathPowInvocationExpression()));

        public static bool DoesNotUseAddSeconds(this GigasecondSolution gigasecondSolution) =>
            !gigasecondSolution.AddMethod.InvokesExpression(
                GigasecondAddSecondsMemberAccessExpression(gigasecondSolution));

        public static bool CreatesNewDatetime(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddMethod.CreatesObjectOfType<DateTime>();
        
        public static ArgumentSyntax AddSecondsArgument(this ExpressionSyntax expression, ParameterSyntax parameter)
        {
            if (expression is InvocationExpressionSyntax invocationExpression &&
                invocationExpression.Expression.IsEquivalentWhenNormalized(
                    SimpleMemberAccessExpression(
                        IdentifierName(parameter),
                        IdentifierName("AddSeconds"))))
                return invocationExpression.ArgumentList.Arguments[0];

            return null;
        }
    }
}