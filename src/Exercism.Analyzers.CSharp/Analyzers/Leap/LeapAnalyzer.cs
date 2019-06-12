using System.Collections.Generic;
using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Shared;

namespace Exercism.Analyzers.CSharp.Analyzers.Leap
{
    internal static class LeapAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution) =>
            Analyze(LeapSolutionParser.Parse(parsedSolution));

        private static SolutionAnalysis Analyze(LeapSolution twoFerSolution) =>
            twoFerSolution.DisapproveWhenInvalid() ??
            twoFerSolution.ApproveWhenValid() ??
            twoFerSolution.ReferToMentor();

        private static SolutionAnalysis DisapproveWhenInvalid(this LeapSolution leapSolution)
        {
            var comments = new List<string>();

            if (leapSolution.UsesDateTimeIsLeapYear())
                 comments.Add(LeapComments.DoNotUseIsLeapYear);

            if (leapSolution.UsesTooManyChecks())
                comments.Add(LeapComments.UseMinimumNumberOfChecks);

            return comments.Any() ? leapSolution.DisapproveWithComment(comments.ToArray()) : null;
        }

        private static SolutionAnalysis ApproveWhenValid(this LeapSolution leapSolution)
        {
            var comments = new List<string>();

            if (!leapSolution.UsesExpressionBody())
                comments.Add(SharedComments.UseExpressionBodiedMember);

            if (comments.Any())
                return leapSolution.ApproveWithComment(comments.ToArray());

            if (leapSolution.ReturnsMinimumNumberOfChecksInSingleExpression())
                return leapSolution.ApproveAsOptimal();

            return null;
        }
    }
}