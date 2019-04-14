using System;
using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Gigasecond.GigasecondSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal static class GigasecondSyntax
    {
        public static bool UsesExpressionBody(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddMethod.IsExpressionBody();

        public static bool ReturnsAddSecondsWithScientificNotation(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.Returns(
                GigasecondAddSecondsWithScientificNotationInvocationExpression(gigasecondSolution));

        public static bool ReturnsAddSecondsWithDigitsWithoutSeparator(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.Returns(
                GigasecondAddSecondsWithDigitsWithoutSeparatorInvocationExpression(gigasecondSolution));

        public static bool ReturnsAddSecondsWithDigitsWithSeparator(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.Returns(
                GigasecondAddSecondsWithDigitsWithSeparatorInvocationExpression(gigasecondSolution));

        public static bool ReturnsAddSecondsWithMathPow(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.Returns(
                GigasecondAddSecondsInvocationExpression(
                    gigasecondSolution,
                    GigasecondAddSecondsWithMathPowInvocationExpression()));
        public static bool AssignsAndReturnsAddSecondsWithScientificNotation(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AssignsAndReturns(
                GigasecondAddSecondsWithScientificNotationInvocationExpression(gigasecondSolution));

        public static bool AssignsAndReturnsAddSecondsWithDigitsWithoutSeparator(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AssignsAndReturns(
                GigasecondAddSecondsWithDigitsWithoutSeparatorInvocationExpression(gigasecondSolution));

        public static bool AssignsAndReturnsAddSecondsWithDigitsWithSeparator(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AssignsAndReturns(
                GigasecondAddSecondsWithDigitsWithSeparatorInvocationExpression(gigasecondSolution));

        public static bool AssignsAndReturnsAddSecondsWithMathPow(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AssignsAndReturns(
                GigasecondAddSecondsInvocationExpression(
                    gigasecondSolution,
                    GigasecondAddSecondsWithMathPowInvocationExpression()));

        public static bool UsesAddMethod(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddMethod.InvokesExpression(
                GigasecondAddMemberAccessExpression(gigasecondSolution));

        public static bool UsesPlusOperator(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddMethod
                .DescendantNodes<BinaryExpressionSyntax>()
                .Any(binaryExpression =>
                        binaryExpression.IsKind(SyntaxKind.AddExpression) &&
                        binaryExpression.DescendantNodes<IdentifierNameSyntax>()
                            .Any(identifierName =>
                                identifierName.IsEquivalentWhenNormalized(
                                    GigasecondParameterIdentifierName(gigasecondSolution))));

        public static bool CreatesDatetime(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddMethod.CreatesObjectOfType<DateTime>();

        public static bool AssignsAndReturnsVariable(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.ReturnedVariableExpression != null;
    }
}