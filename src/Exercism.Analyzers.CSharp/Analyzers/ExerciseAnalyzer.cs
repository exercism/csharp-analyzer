using System.Collections.Generic;
using System.Linq;

using Exercism.Representers.CSharp;

using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal abstract class ExerciseAnalyzer<T> where T : Solution
{
    private readonly List<SolutionComment> _comments = new();
    private readonly SortedSet<string> _tags = new();

    protected void AddComment(SolutionComment comment) =>
        _comments.Add(comment);

    protected bool HasComments => _comments.Any();

    protected SolutionAnalysis Analysis => new(_comments.ToArray(), _tags.ToArray());

    protected SolutionAnalysis AnalysisWithComment(SolutionComment comment) => new(new[] { comment }, _tags.ToArray());

    public SolutionAnalysis Analyze(T solution)
    {
        new IdentifyTags(_tags).Visit(solution.SyntaxRoot);

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