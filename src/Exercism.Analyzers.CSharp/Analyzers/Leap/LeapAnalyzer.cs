using Exercism.Analyzers.CSharp.Analyzers.Shared;

namespace Exercism.Analyzers.CSharp.Analyzers.Leap
{
    internal static class LeapAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution) =>
            Analyze(new LeapSolution(parsedSolution));

        private static SolutionAnalysis Analyze(LeapSolution leapSolution)
        {
            if (leapSolution.UsesTooManyChecks())
                return leapSolution.DisapproveWithComment(LeapComments.UseMinimumNumberOfChecks);
            
            if (leapSolution.ReturnsMinimumNumberOfChecksInSingleExpression())
                return leapSolution.UsesExpressionBody()
                    ? leapSolution.ApproveAsOptimal()
                    : leapSolution.ApproveWithComment(SharedComments.UseExpressionBodiedMember);

            return leapSolution.ReferToMentor();
        }
    }
}