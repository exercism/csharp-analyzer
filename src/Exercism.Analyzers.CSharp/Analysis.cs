using System.Collections.Generic;

using Exercism.Analyzers.CSharp.Analyzers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Exercism.Analyzers.CSharp;

internal record Analysis(List<Comment> Comments, List<string> Tags)
{
    public static Analysis Empty => new(new List<Comment>(), new List<string>());

    public void AddComment(Comment comment) => Comments.Add(comment);
    public void AddTag(string tag) => Tags.Add(tag);
}

internal abstract class Analyzer : CSharpSyntaxWalker
{
    protected readonly Analysis _analysis;
    protected readonly Compilation _compilation;

    protected Analyzer(Compilation compilation, Analysis analysis) =>
        (_compilation, _analysis) = (compilation, analysis);

    private void Analyze()
    {
        foreach (var syntaxTree in _compilation.SyntaxTrees)
            Visit(syntaxTree.GetRoot());
    }
    
    public static Analysis Analyze(Solution solution)
    {
        if (solution.HasCompilationErrors)
        {
            return Analysis.Empty; // We can't really analyze the solution when there are compilation errors
        }

        var analysis = Analysis.Empty;

        foreach (var analyzer in CreateAnalyzers(solution, analysis))
            analyzer.Analyze();

        return analysis;
    }

    private static IEnumerable<Analyzer> CreateAnalyzers(Solution solution, Analysis analysis)
    {
        switch (solution.Slug)
        {
            case "leap":
                yield return new LeapAnalyzer(solution.Compilation, analysis);
                break;
            case "gigasecond":
                yield return new GigasecondAnalyzer(solution.Compilation, analysis);
                break;
            case "two-fer":
                yield return new TwoFerAnalyzer(solution.Compilation, analysis);
                break;
            case "weighing-machine":
                yield return new WeighingMachineAnalyzer(solution.Compilation, analysis);
                break;
        }

        yield return new CommonAnalyzer(solution.Compilation, analysis);
        yield return new TagAnalyzer(solution.Compilation, analysis);
    }
}
