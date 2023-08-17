using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal abstract class ExerciseAnalyzer<T> where T : Solution
{
    public SolutionAnalysis Analyze(T solution)
    {
        if (solution.NoImplementationFileFound())
            return solution.Analysis;

        if (solution.HasCompileErrors())
            solution.AddComment(HasCompileErrors);

        if (solution.HasMainMethod())
            solution.AddComment(HasMainMethod);

        if (solution.ThrowsNotImplementedException())
            solution.AddComment(RemoveThrowNotImplementedException);

        if (solution.WritesToConsole())
            solution.AddComment(DoNotWriteToConsole);

        if (solution.HasComments)
            return solution.Analysis;

        return AnalyzeSpecific(solution);
    }

    protected abstract SolutionAnalysis AnalyzeSpecific(T solution);
}