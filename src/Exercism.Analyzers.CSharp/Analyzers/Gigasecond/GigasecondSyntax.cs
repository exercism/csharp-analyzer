using System;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis;
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

        public static bool UsesAddSecondsWithDigitsWithoutSeparatorVariable(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.UsesAddSecondsWithVariable(GigasecondDigitsWithoutSeparator());

        public static bool UsesAddSecondsWithDigitsWithSeparator(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.Returns(
                GigasecondAddSecondsWithDigitsWithSeparatorInvocationExpression(gigasecondSolution));

        public static bool UsesAddSecondsWithDigitsWithSeparatorVariable(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.UsesAddSecondsWithVariable(GigasecondDigitsWithSeparator());

        public static bool UsesAddSecondsWithMathPow(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.Returns(
                GigasecondAddSecondsInvocationExpression(
                    gigasecondSolution,
                    GigasecondMathPowInvocationExpression()));

        public static bool UsesAddSecondsWithMathPowVariable(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.UsesAddSecondsWithVariable(GigasecondMathPowInvocationExpression());

        private static bool UsesAddSecondsWithVariable(this GigasecondSolution gigasecondSolution, ExpressionSyntax initializer) =>
            gigasecondSolution.UsesAddSecondsWithVariableArgument() &&
            gigasecondSolution.AddSecondsArgumentVariable.Initializer.Value.IsEquivalentWhenNormalized(initializer);

        private static bool UsesAddSecondsWithVariableArgument(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.UsesVariableInAddSecondsInvocation &&
            gigasecondSolution.Returns(
                GigasecondAddSecondsInvocationExpression(
                    gigasecondSolution,
                    gigasecondSolution.AddSecondsArgumentName));

        public static bool DoesNotUseAddSeconds(this GigasecondSolution gigasecondSolution) =>
            !gigasecondSolution.AddMethod.InvokesExpression(
                GigasecondAddSecondsMemberAccessExpression(gigasecondSolution));

        public static bool CreatesNewDatetime(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddMethod.CreatesObjectOfType<DateTime>();
        
        public static IdentifierNameSyntax AddSecondsArgumentName(this ExpressionSyntax expression, ParameterSyntax parameter)
        {
            if (expression is InvocationExpressionSyntax invocationExpression &&
                invocationExpression.Expression.IsEquivalentWhenNormalized(
                    SimpleMemberAccessExpression(
                        IdentifierName(parameter),
                        IdentifierName("AddSeconds"))))
                return invocationExpression.ArgumentList.Arguments[0].Expression as IdentifierNameSyntax;

            return null;
        }

        public static ExpressionSyntax ExpressionUsesVariableAndReturned(this MethodDeclarationSyntax nameMethod)
        {
            if (nameMethod?.Body?.Statements.Count != 2)
                return null;

            var localDeclaration = nameMethod.Body.Statements[0] as LocalDeclarationStatementSyntax;
            var returnStatement = nameMethod.Body.Statements[1] as ReturnStatementSyntax;
            
            if (localDeclaration == null || returnStatement == null)
                return null;

            if (localDeclaration.Declaration.Variables.Count != 1 ||
                !returnStatement.UsesVariableAsArgument(localDeclaration.Declaration.Variables[0]))
                return null;

            return returnStatement.Expression;
        }
    }
}