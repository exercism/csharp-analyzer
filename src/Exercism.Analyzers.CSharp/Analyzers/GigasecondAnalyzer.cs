using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.GigasecondComments;
using static Exercism.Analyzers.CSharp.Analyzers.SharedComments;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class GigasecondAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution)
        {
            var gigasecondSolution = new GigasecondSolution(parsedSolution);

            if (gigasecondSolution.UsesAddSecondsMethodWithScientificNotation())
                return gigasecondSolution.AddMethod.IsExpressionBody()
                    ? gigasecondSolution.ApproveAsOptimal()
                    : gigasecondSolution.ApproveWithComment(UseExpressionBodiedMember);

            if (gigasecondSolution.UsesAddSecondsMethodWithMathPow())
                return gigasecondSolution.ApproveWithComment(UseScientificNotationNotMathPow);

            if (gigasecondSolution.UsesAddSecondsMethodWithDigitsWithoutSeparator())
                return gigasecondSolution.ApproveWithComment(UseScientificNotationOrDigitSeparators);

            if (gigasecondSolution.UsesAddMethod() ||
                gigasecondSolution.UsesPlusOperator())
                return gigasecondSolution.DisapproveWithComment(UseAddSeconds);

            return gigasecondSolution.ReferToMentor();
        }

        private static bool UsesAddSecondsMethodWithScientificNotation(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution
                .UsesAddSecondsMethodWithArgument(
                    LiteralExpression(
                        SyntaxKind.NumericLiteralExpression,
                        Literal("1e9", 1e9))) ||
            gigasecondSolution
                .UsesAddSecondsMethodWithArgument(
                    LiteralExpression(
                        SyntaxKind.NumericLiteralExpression,
                        Literal("1E9", 1e9)));

        private static bool UsesAddSecondsMethodWithDigitsWithoutSeparator(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution
                .UsesAddSecondsMethodWithArgument(
                    LiteralExpression(
                        SyntaxKind.NumericLiteralExpression,
                        Literal(1000000000)));

        private static bool UsesAddSecondsMethodWithMathPow(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution
                .UsesAddSecondsMethodWithArgument(
                    InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName("Math"),
                                IdentifierName("Pow")))
                        .WithArgumentList(
                            ArgumentList(
                                SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]{
                                        Argument(
                                            LiteralExpression(
                                                SyntaxKind.NumericLiteralExpression,
                                                Literal(10))),
                                        Token(SyntaxKind.CommaToken),
                                        Argument(
                                            LiteralExpression(
                                                SyntaxKind.NumericLiteralExpression,
                                                Literal(9)))}))));
        
        private static bool UsesAddSecondsMethodWithArgument(this GigasecondSolution gigasecondSolution, ExpressionSyntax argumentExpression) =>
            gigasecondSolution.AddMethod.InvokesExpression(
                InvocationExpression(
                        gigasecondSolution.BirthDateParameter.ToMemberAccessExpression("AddSeconds"))
                    .WithArgumentList(
                        ArgumentList(
                            SingletonSeparatedList(
                                Argument(argumentExpression)))));

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
                                identifierName.IsSafeEquivalentTo(
                                    IdentifierName(gigasecondSolution.BirthDateParameter.Identifier))));

        private class GigasecondSolution : ParsedSolution
        {
            public MethodDeclarationSyntax AddMethod { get; }
            public ParameterSyntax BirthDateParameter { get; }

            public GigasecondSolution(ParsedSolution solution) : base(solution.Solution, solution.SyntaxRoot)
            {
                AddMethod = solution.SyntaxRoot.GetClassMethod("Gigasecond","Add");
                BirthDateParameter = AddMethod.ParameterList.Parameters[0];
            }
        }
    }
}