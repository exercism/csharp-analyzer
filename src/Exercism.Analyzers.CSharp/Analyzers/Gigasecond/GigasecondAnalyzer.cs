using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Shared;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal static class GigasecondAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution)
        {
            var gigasecondSolution = new GigasecondSolution(parsedSolution);

            if (gigasecondSolution.UsesAddSecondsMethodWithScientificNotation())
                return gigasecondSolution.AddMethod.IsExpressionBody()
                    ? gigasecondSolution.ApproveAsOptimal()
                    : gigasecondSolution.ApproveWithComment(SharedComments.UseExpressionBodiedMember);

            if (gigasecondSolution.UsesAddSecondsMethodWithMathPow())
                return gigasecondSolution.ApproveWithComment(GigasecondComments.UseScientificNotationNotMathPow);

            if (gigasecondSolution.UsesAddSecondsMethodWithDigitsWithoutSeparator())
                return gigasecondSolution.ApproveWithComment(GigasecondComments.UseScientificNotationOrDigitSeparators);

            if (gigasecondSolution.UsesAddMethod() ||
                gigasecondSolution.UsesPlusOperator())
                return gigasecondSolution.DisapproveWithComment(GigasecondComments.UseAddSeconds);

            return gigasecondSolution.ReferToMentor();
        }

        private static bool UsesAddSecondsMethodWithScientificNotation(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution
                .UsesAddSecondsMethodWithArgument(
                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.NumericLiteralExpression,
                        SyntaxFactory.Literal("1e9", 1e9))) ||
            gigasecondSolution
                .UsesAddSecondsMethodWithArgument(
                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.NumericLiteralExpression,
                        SyntaxFactory.Literal("1E9", 1e9)));

        private static bool UsesAddSecondsMethodWithDigitsWithoutSeparator(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution
                .UsesAddSecondsMethodWithArgument(
                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.NumericLiteralExpression,
                        SyntaxFactory.Literal(1000000000)));

        private static bool UsesAddSecondsMethodWithMathPow(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution
                .UsesAddSecondsMethodWithArgument(
                    SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("Math"),
                                SyntaxFactory.IdentifierName("Pow")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]{
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.NumericLiteralExpression,
                                                SyntaxFactory.Literal(10))),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.NumericLiteralExpression,
                                                SyntaxFactory.Literal(9)))}))));

        private static bool UsesAddSecondsMethodWithArgument(this GigasecondSolution gigasecondSolution, ExpressionSyntax argumentExpression) =>
            gigasecondSolution.AddMethod.InvokesExpression(
                SyntaxFactory.InvocationExpression(
                        gigasecondSolution.BirthDateParameter.ToMemberAccessExpression("AddSeconds"))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList(
                                SyntaxFactory.Argument(argumentExpression)))));

        private static bool UsesAddMethod(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddMethod.InvokesExpression(
                gigasecondSolution.BirthDateParameter.ToMemberAccessExpression("Add"));

        private static bool UsesPlusOperator(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddMethod
                .DescendantNodes<BinaryExpressionSyntax>()
                .Any(binaryExpression =>
                        binaryExpression.IsKind(SyntaxKind.AddExpression) &&
                        binaryExpression
                            .DescendantNodes<IdentifierNameSyntax>()
                            .Any(identifierName =>
                                identifierName.IsEquivalentWhenNormalized(
                                    SyntaxFactory.IdentifierName(gigasecondSolution.BirthDateParameter.Identifier))));

        private class GigasecondSolution : ParsedSolution
        {
            public MethodDeclarationSyntax AddMethod { get; }
            public ParameterSyntax BirthDateParameter { get; }

            public GigasecondSolution(ParsedSolution solution) : base(solution.Solution, solution.SyntaxRoot)
            {
                AddMethod = solution.SyntaxRoot.GetClassMethod("Gigasecond", "Add");
                BirthDateParameter = AddMethod.ParameterList.Parameters[0];
            }
        }
    }
}