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
    protected Analysis _analysis;
    protected Compilation _compilation;
    
    public static Analysis Analyze(Solution solution)
    {
        if (solution.HasCompilationErrors)
            return Analysis.Empty; // We can't really analyze the solution when there are compilation errors

        var analysis = Analysis.Empty;

        foreach (var analyzer in CreateAnalyzers(solution.Slug))
            analyzer.Analyze(solution.Compilation, analysis);

        return analysis;
    }

    private void Analyze(Compilation compilation, Analysis analysis)
    {
        _compilation = compilation;
        _analysis = analysis;
        
        foreach (var syntaxTree in _compilation.SyntaxTrees)
            Visit(syntaxTree.GetRoot());
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
