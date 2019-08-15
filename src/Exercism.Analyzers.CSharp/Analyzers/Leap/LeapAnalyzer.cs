using Exercism.Analyzers.CSharp.Analyzers.Shared;
using static Exercism.Analyzers.CSharp.Analyzers.Leap.LeapComments;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;

namespace Exercism.Analyzers.CSharp.Analyzers.Leap
{
    internal class LeapAnalyzer : SharedAnalyzer<LeapSolution>
    {
        protected override SolutionAnalysis DisapproveWhenInvalid(LeapSolution solution)
        {
            if (solution.UsesDateTimeIsLeapYear)
                solution.AddComment(DoNotUseIsLeapYear);

            if (solution.UsesNestedIfStatement)
                solution.AddComment(DoNotUseNestedIfStatement);

            if (solution.UsesTooManyChecks)
                solution.AddComment(UseMinimumNumberOfChecks);

            return solution.HasComments
                ? solution.Disapprove()
                : solution.ContinueAnalysis();
        }

        protected override SolutionAnalysis ApproveWhenValid(LeapSolution solution)
        {
            if (solution.UsesIfStatement)
                solution.AddComment(DoNotUseIfStatement);

            if (solution.UsesSingleLine && !solution.UsesExpressionBody)
                solution.AddComment(UseExpressionBodiedMember(solution.IsLeapYearMethodName));

            if (solution.ReturnsMinimumNumberOfChecksInSingleExpression ||
                solution.HasComments)
                return solution.Approve();

            return solution.ContinueAnalysis();
        }
    }
}