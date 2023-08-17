using System.Collections.Generic;
using System.Linq;

using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal abstract class ExerciseAnalyzer<T> where T : Solution
{
    private readonly List<SolutionComment> _comments = new();

    public void AddComment(SolutionComment comment) =>
        _comments.Add(comment);

    public bool HasComments => _comments.Any();

    public SolutionAnalysis Analysis => new(_comments.ToArray());

    public SolutionAnalysis AnalysisWithComment(SolutionComment comment) => new(new[] { comment });

    public SolutionAnalysis Analyze(T solution)
    {
        if (solution.NoImplementationFileFound())
            return Analysis;

        if (solution.HasCompileErrors())
            return AnalysisWithComment(HasCompileErrors);

        if (solution.HasMainMethod())
            return AnalysisWithComment(HasMainMethod);

        if (solution.ThrowsNotImplementedException())
            return AnalysisWithComment(RemoveThrowNotImplementedException);

        if (solution.WritesToConsole())
            return AnalysisWithComment(DoNotWriteToConsole);

        if (HasComments)
            return Analysis;

        return AnalyzeSpecific(solution);
    }

    protected abstract SolutionAnalysis AnalyzeSpecific(T solution);
}