using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Shared;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Leap
{
    internal static class LeapAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution)
        {
            var leapSolution = new LeapSolution(parsedSolution);

            if (leapSolution.UsesTooManyChecks())
                return leapSolution.DisapproveWithComment(LeapComments.UseMinimumNumberOfChecks);

            if (leapSolution.IsLeapYearMethod.IsExpressionBody())
                return leapSolution.ApproveAsOptimal();

            if (leapSolution.IsLeapYearMethod.CanConvertToExpressionBody())
                return leapSolution.ApproveWithComment(SharedComments.UseExpressionBodiedMember);

            return leapSolution.ReferToMentor();
        }

        private static bool UsesTooManyChecks(this LeapSolution leapSolution)
        {
            const int minimalNumberOfChecks = 3;

            return leapSolution.IsLeapYearMethod
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .Count(BinaryExpressionUsesYearParameter) > minimalNumberOfChecks;

            bool BinaryExpressionUsesYearParameter(BinaryExpressionSyntax binaryExpression) =>
                binaryExpression.Left.IsEquivalentWhenNormalized(SyntaxFactory.IdentifierName(leapSolution.YearParameter.Identifier)) ||
                binaryExpression.Right.IsEquivalentWhenNormalized(SyntaxFactory.IdentifierName(leapSolution.YearParameter.Identifier));
        }

        private class LeapSolution : ParsedSolution
        {
            public MethodDeclarationSyntax IsLeapYearMethod { get; }
            public ParameterSyntax YearParameter { get; }

            public LeapSolution(ParsedSolution solution) : base(solution.Solution, solution.SyntaxRoot)
            {
                IsLeapYearMethod = solution.SyntaxRoot.GetClassMethod("Leap", "IsLeapYear");
                YearParameter = IsLeapYearMethod.ParameterList.Parameters[0];
            }
        }
    }
}