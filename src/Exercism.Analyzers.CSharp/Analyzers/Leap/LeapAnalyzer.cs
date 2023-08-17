using static Exercism.Analyzers.CSharp.Analyzers.Leap.LeapComments;
using static Exercism.Analyzers.CSharp.Analyzers.Leap.LeapSolution;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;

namespace Exercism.Analyzers.CSharp.Analyzers.Leap;

internal class LeapAnalyzer : ExerciseAnalyzer<LeapSolution>
{
    protected override SolutionAnalysis AnalyzeSpecific(LeapSolution solution)
    {
        if (solution.MissingLeapClass)
            solution.AddComment(MissingClass(LeapClassName));

        if (solution.MissingIsLeapYearMethod)
            solution.AddComment(MissingMethod(IsLeapYearMethodName));

        if (solution.InvalidIsLeapYearMethod)
            solution.AddComment(InvalidMethodSignature(IsLeapYearMethodName, IsLeapYearMethodSignature));

        if (solution.UsesDateTimeIsLeapYear)
            solution.AddComment(DoNotUseIsLeapYear);

        if (solution.UsesNestedIfStatement)
            solution.AddComment(DoNotUseNestedIfStatement);

        if (solution.UsesTooManyChecks)
            solution.AddComment(UseMinimumNumberOfChecks);

        if (solution.UsesIfStatement)
            solution.AddComment(DoNotUseIfStatement);

        if (solution.UsesSingleLine && !solution.UsesExpressionBody)
            solution.AddComment(UseExpressionBodiedMember(IsLeapYearMethodName));

        return solution.Analysis;
    }
}