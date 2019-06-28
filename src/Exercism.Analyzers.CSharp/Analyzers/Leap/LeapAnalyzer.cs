using static Exercism.Analyzers.CSharp.Analyzers.Leap.LeapComments;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedCommentParameters;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;

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
                leapSolution.AddComment(DoNotUseIsLeapYear);

            if (leapSolution.UsesNestedIfStatement())
                leapSolution.AddComment(DoNotUseNestedIfStatement);

            if (leapSolution.UsesTooManyChecks())
                leapSolution.AddComment(UseMinimumNumberOfChecks);

            return leapSolution.HasComments()
                ? leapSolution.DisapproveWithComment()
                : leapSolution.ContinueAnalysis();
        }

        private static SolutionAnalysis ApproveWhenValid(this LeapSolution leapSolution)
        {
            if (leapSolution.UsesIfStatement())
                leapSolution.AddComment(DoNotUseIfStatement);

            if (leapSolution.UsesSingleLine() && !leapSolution.UsesExpressionBody())
                leapSolution.AddComment(UseExpressionBodiedMember, new SolutionCommentParameter(Method, leapSolution.IsLeapYearMethodName));

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