using static Exercism.Analyzers.CSharp.Analyzers.Leap.LeapComments;
using static Exercism.Analyzers.CSharp.Analyzers.Leap.LeapSolution;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;

namespace Exercism.Analyzers.CSharp.Analyzers.Leap;

internal class LeapAnalyzer : ExerciseAnalyzer<LeapSolution>
{
    protected override SolutionAnalysis AnalyzeSpecific(LeapSolution solution)
    {
        if (solution.MissingLeapClass)
            return solution.AnalysisWithComment(MissingClass(LeapClassName));

        if (solution.MissingIsLeapYearMethod)
            return solution.AnalysisWithComment(MissingMethod(IsLeapYearMethodName));

        if (solution.InvalidIsLeapYearMethod)
            return solution.AnalysisWithComment(InvalidMethodSignature(IsLeapYearMethodName, IsLeapYearMethodSignature));

        if (solution.UsesDateTimeIsLeapYear)
            solution.AddComment(DoNotUseIsLeapYear);

        if (solution.UsesNestedIfStatement)
            solution.AddComment(DoNotUseNestedIfStatement);
        else if (solution.UsesIfStatement)
            solution.AddComment(DoNotUseIfStatement);

        if (solution.UsesTooManyChecks)
            solution.AddComment(UseMinimumNumberOfChecks);

        if (solution.UsesSingleLine && !solution.UsesExpressionBody)
            solution.AddComment(UseExpressionBodiedMember(IsLeapYearMethodName));

        return solution.Analysis;
    }
}