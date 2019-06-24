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
            twoFerSolution.ApproveWhenOptimal() ??
            twoFerSolution.ReferToMentor();

        private static SolutionAnalysis DisapproveWhenInvalid(this LeapSolution leapSolution)
        {
            if (leapSolution.UsesDateTimeIsLeapYear())
                leapSolution.AddComment(LeapComments.DoNotUseIsLeapYear);

            if (leapSolution.UsesNestedIfStatement())
                leapSolution.AddComment(LeapComments.DoNotUseNestedIfStatement);

            if (leapSolution.UsesTooManyChecks())
                leapSolution.AddComment(LeapComments.UseMinimumNumberOfChecks);

            return leapSolution.HasComments()
                ? leapSolution.DisapproveWithComment()
                : leapSolution.ContinueAnalysis();
        }

        private static SolutionAnalysis ApproveWhenValid(this LeapSolution leapSolution)
        {
            if (leapSolution.UsesIfStatement())
                leapSolution.AddComment(LeapComments.DoNotUseIfStatement);

            if (leapSolution.UsesSingleLine() && !leapSolution.UsesExpressionBody())
                leapSolution.AddComment(SharedComments.UseExpressionBodiedMember);

            return leapSolution.HasComments()
                ? leapSolution.ApproveWithComment()
                : leapSolution.ContinueAnalysis();
        }

        private static SolutionAnalysis ApproveWhenOptimal(this LeapSolution leapSolution)
        {
            if (leapSolution.ReturnsMinimumNumberOfChecksInSingleExpression())
                return leapSolution.ApproveAsOptimal();

            return leapSolution.ContinueAnalysis();
        }
    }
}