using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;

namespace Exercism.Analyzers.CSharp.Analyzers.Shared;

internal abstract class SharedAnalyzer<T> where T : Solution
{
    public SolutionAnalysis Analyze(T solution) =>
        AnalyzeShared(solution) ?? AnalyzeSpecific(solution);

    private SolutionAnalysis AnalyzeShared(T solution)
    {
        if (solution.NoImplementationFileFound())
            return solution.ReferToMentor();

        if (solution.HasCompileErrors())
            solution.AddComment(HasCompileErrors);

        if (solution.HasMainMethod())
            solution.AddComment(HasMainMethod);

        if (solution.ThrowsNotImplementedException())
            solution.AddComment(RemoveThrowNotImplementedException);

        if (solution.WritesToConsole())
            solution.AddComment(DoNotWriteToConsole);

        if (solution.HasComments)
            return solution.Disapprove();

        return null;
    }

    protected abstract SolutionAnalysis AnalyzeSpecific(T solution);
}