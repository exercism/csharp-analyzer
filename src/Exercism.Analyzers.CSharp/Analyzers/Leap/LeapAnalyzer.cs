using static Exercism.Analyzers.CSharp.Analyzers.Leap.LeapComments;
using static Exercism.Analyzers.CSharp.Analyzers.Leap.LeapSolution;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;

namespace Exercism.Analyzers.CSharp.Analyzers.Leap;

internal class LeapAnalyzer : ExerciseAnalyzer<LeapSolution>
{
    protected override SolutionAnalysis AnalyzeSpecific(LeapSolution solution)
    {
        if (solution.MissingLeapClass)
            return AnalysisWithComment(MissingClass(LeapClassName));

        if (solution.MissingIsLeapYearMethod)
            return AnalysisWithComment(MissingMethod(IsLeapYearMethodName));

        if (solution.InvalidIsLeapYearMethod)
            return AnalysisWithComment(InvalidMethodSignature(IsLeapYearMethodName, IsLeapYearMethodSignature));

        if (solution.UsesDateTimeIsLeapYear)
            AddComment(DoNotUseIsLeapYear);

        if (solution.UsesNestedIfStatement)
            AddComment(DoNotUseNestedIfStatement);
        else if (solution.UsesIfStatement)
            AddComment(DoNotUseIfStatement);

        if (solution.UsesTooManyChecks)
            AddComment(UseMinimumNumberOfChecks);

        if (solution.CanUseExpressionBody)
            AddComment(UseExpressionBodiedMember(IsLeapYearMethodName));

        return Analysis;
    }
}