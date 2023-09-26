using System.Collections.Generic;

using Exercism.Analyzers.CSharp.Analyzers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Exercism.Analyzers.CSharp;

internal record Analysis(List<Comment> Comments, List<string> Tags)
{
    public static Analysis Empty => new(new List<Comment>(), new List<string>());
}

internal abstract class Analyzer : CSharpSyntaxWalker
{
    protected Submission Submission;
    protected Compilation Compilation => Submission.Compilation;
    protected Project Project => Submission.Project;
    protected Solution Solution => Project.Solution;
    protected SemanticModel SemanticModel;
    protected Analysis Analysis;

    protected Analyzer(Submission submission) => Submission = submission;

    public static Analysis Analyze(Submission submission)
    {
        if (submission.HasCompilationErrors)
            return Analysis.Empty; // We can't really analyze the solution when there are compilation errors

        var analysis = Analysis.Empty;

        foreach (var analyzer in CreateAnalyzers(submission))
            analyzer.Analyze(analysis);

        return analysis;
    }

    protected void AddComment(Comment comment) => Analysis.Comments.Add(comment);

    protected void AddTags(params string[] tags)
    {
        foreach (var tag in tags)
            Analysis.Tags.Add(tag);
    }

    private void Analyze(Analysis analysis)
    {
        Analysis = analysis;

        foreach (var syntaxTree in Compilation.SyntaxTrees)
        {
            SemanticModel = Compilation.GetSemanticModel(syntaxTree);
            Visit(syntaxTree.GetRoot());
        }
    }

    private static IEnumerable<Analyzer> CreateAnalyzers(Submission submission)
    {
        if (submission.Slug == "leap")
            yield return new LeapAnalyzer(submission);
        else if (submission.Slug == "gigasecond")
            yield return new GigasecondAnalyzer(submission);
        else if (submission.Slug == "two-fer")
            yield return new TwoFerAnalyzer(submission);
        else if (submission.Slug == "weighing-machine")
            yield return new WeighingMachineAnalyzer(submission);

        yield return new CommonAnalyzer(submission);
        yield return new TagAnalyzer(submission);
    }
}