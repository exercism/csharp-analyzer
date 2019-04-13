using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Gigasecond.GigasecondSyntaxFactory;

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
    }
}