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
    protected Analysis Analysis;
    protected Compilation Compilation;
    protected SemanticModel SemanticModel;

    public static Analysis Analyze(Solution solution)
    {
        if (solution.HasCompilationErrors)
            return Analysis.Empty; // We can't really analyze the solution when there are compilation errors

        var analysis = Analysis.Empty;

        foreach (var analyzer in CreateAnalyzers(solution.Slug))
            analyzer.Analyze(solution.Compilation, analysis);

        return analysis;
    }

    protected void AddComment(Comment comment) => Analysis.Comments.Add(comment);
    protected void AddTag(string tag) => Analysis.Tags.Add(tag);

    private void Analyze(Compilation compilation, Analysis analysis)
    {
        Compilation = compilation;
        Analysis = analysis;

        foreach (var syntaxTree in Compilation.SyntaxTrees)
        {
            SemanticModel = Compilation.GetSemanticModel(syntaxTree);
            Visit(syntaxTree.GetRoot());
        }
    }

    private static IEnumerable<Analyzer> CreateAnalyzers(string slug)
    {
        if (slug == "leap")
            yield return new LeapAnalyzer();
        else if (slug == "gigasecond")
            yield return new GigasecondAnalyzer();
        else if (slug == "two-fer")
            yield return new TwoFerAnalyzer();
        else if (slug == "weighing-machine")
            yield return new WeighingMachineAnalyzer();

        yield return new CommonAnalyzer();
        yield return new TagAnalyzer();
    }
}