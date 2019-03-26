using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.LeapComments;
using static Exercism.Analyzers.CSharp.Analyzers.SharedComments;

namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class LeapAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution)
        {
            var leapSolution = new LeapSolution(parsedSolution);
            
            if (leapSolution.UsesTooManyChecks())
                return leapSolution.DisapproveWithComment(UseMinimumNumberOfChecks);

            if (leapSolution.IsLeapYearMethod.IsExpressionBody())
                return leapSolution.ApproveAsOptimal();

            if (leapSolution.IsLeapYearMethod.CanConvertToExpressionBody())
                return leapSolution.ApproveWithComment(UseExpressionBodiedMember);

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
                binaryExpression.Left.IsSafeEquivalentTo(SyntaxFactory.IdentifierName(leapSolution.YearParameter.Identifier)) ||
                binaryExpression.Right.IsSafeEquivalentTo(SyntaxFactory.IdentifierName(leapSolution.YearParameter.Identifier));
        }

        private class LeapSolution : ParsedSolution
        {
            public MethodDeclarationSyntax IsLeapYearMethod { get; }
            public ParameterSyntax YearParameter { get; }

            public LeapSolution(ParsedSolution solution) : base(solution.Solution, solution.SyntaxRoot)
            {
                IsLeapYearMethod = solution.SyntaxRoot.GetClassMethod("Leap","IsLeapYear");
                YearParameter = IsLeapYearMethod.ParameterList.Parameters[0];
            }
        }
    }
}