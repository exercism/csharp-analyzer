using System.Collections.Generic;
using System.Linq;

using Exercism.Analyzers.CSharp.Analyzers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Exercism.Analyzers.CSharp;

internal record Solution(string Slug, Compilation Compilation);

internal record Analysis(List<Comment> Comments, List<string> Tags)
{
    public static Analysis Empty => new(new List<Comment>(), new List<string>());

    public void AddComment(Comment comment) => Comments.Add(comment);
    public void AddTag(string tag) => Tags.Add(tag);
}

internal static class Analyzer
{
    public static Analysis Analyze(Solution solution)
    {
        if (solution.HasCompilationErrors())
        {
            return Analysis.Empty; // We can't really analyze the solution when there are compilation errors
        }

        var analysis = Analysis.Empty;
        
        foreach (var analyzer in CreateExerciseAnalyzer(solution, analysis))
        {
            foreach (var syntaxTree in solution.Compilation.SyntaxTrees)
            {
                analyzer.Visit(syntaxTree.GetRoot());
            }
        }

        return analysis;
    }

    private static bool HasCompilationErrors(this Solution solution) =>
        solution.Compilation.GetDiagnostics().Any(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);

    private static IEnumerable<CSharpSyntaxWalker> CreateExerciseAnalyzer(Solution solution, Analysis analysis)
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
        yield return new TagAnalyzer(analysis);
    }
}