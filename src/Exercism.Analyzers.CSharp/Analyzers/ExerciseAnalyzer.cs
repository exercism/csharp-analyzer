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
            AddComment(HasCompileErrors);

        if (solution.HasMainMethod())
            AddComment(HasMainMethod);

        if (solution.ThrowsNotImplementedException())
            AddComment(RemoveThrowNotImplementedException);

        if (solution.WritesToConsole())
            AddComment(DoNotWriteToConsole);

        if (HasComments)
            return Analysis;

        return AnalyzeSpecific(solution);
    }

    protected abstract SolutionAnalysis AnalyzeSpecific(T solution);
}