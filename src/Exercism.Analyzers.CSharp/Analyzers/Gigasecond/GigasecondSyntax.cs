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

        public static bool UsesAddSecondsWithScientificNotationVariable(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.ReturnsAddSecondsUsingVariable(GigasecondScientificNotation());

        public static bool UsesAddSecondsWithDigitsWithoutSeparator(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.Returns(
                GigasecondAddSecondsWithDigitsWithoutSeparatorInvocationExpression(gigasecondSolution));

        public static bool UsesAddSecondsWithDigitsWithoutSeparatorVariable(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.ReturnsAddSecondsUsingVariable(GigasecondDigitsWithoutSeparator());

        public static bool UsesAddSecondsWithDigitsWithSeparator(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.Returns(
                GigasecondAddSecondsWithDigitsWithSeparatorInvocationExpression(gigasecondSolution));

        public static bool UsesAddSecondsWithDigitsWithSeparatorVariable(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.ReturnsAddSecondsUsingVariable(GigasecondDigitsWithSeparator());

        public static bool UsesAddSecondsWithMathPow(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.Returns(
                GigasecondAddSecondsInvocationExpression(
                    gigasecondSolution,
                    GigasecondMathPowInvocationExpression()));

        public static bool UsesAddSecondsWithMathPowVariable(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.ReturnsAddSecondsUsingVariable(GigasecondMathPowInvocationExpression());

        private static bool ReturnsAddSecondsUsingVariable(this GigasecondSolution gigasecondSolution, ExpressionSyntax initializer) =>
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

        public static ExpressionSyntax ExpressionUsesVariableAndReturned(this MethodDeclarationSyntax nameMethod, VariableDeclaratorSyntax variableDeclarator)
        {
            if (nameMethod == null || variableDeclarator == null)
                return null;
            
            var statementsCount = nameMethod.ExpressionBody != null ? 1 : nameMethod.Body.Statements.Count;
            if (statementsCount < 1 || statementsCount > 2)
                return null;

            if (statementsCount == 1)
            {
                if (nameMethod.ExpressionBody != null)
                    return nameMethod.ExpressionBody.UsesVariableAsArgument(variableDeclarator)
                        ? nameMethod.ExpressionBody.Expression
                        : null;
                
                return 
                    nameMethod.Body.Statements[0] is ReturnStatementSyntax singleReturnStatement &&
                    singleReturnStatement.UsesVariableAsArgument(variableDeclarator)
                        ? singleReturnStatement.Expression
                        : null;
            }

            return
                nameMethod.Body.Statements[0] is LocalDeclarationStatementSyntax localDeclaration &&
                nameMethod.Body.Statements[1] is ReturnStatementSyntax returnStatement &&
                localDeclaration.Declaration.Variables.Count == 1 &&
                localDeclaration.Declaration.Variables[0].IsEquivalentWhenNormalized(variableDeclarator) &&
                returnStatement.UsesVariableAsArgument(variableDeclarator)
                    ? returnStatement.Expression
                    : null;
        }
    }
}