using System;
using System.Linq;

using Exercism.Analyzers.CSharp.Exercises;

using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp;

internal record Solution(string Slug, Compilation Compilation);

internal record Analysis(Comment[] Comments, string[] Tags)
{
    public static Analysis Empty = new(Array.Empty<Comment>(), Array.Empty<string>());
}

internal static class Analyzer
{
    public static Analysis Analyze(Solution solution)
    {
        if (solution.HasCompilationErrors())
            return Analysis.Empty; // We can't really analyze the solution when there are compilation errors
        
        return solution.Slug switch
        {
            "gigasecond" => GigasecondAnalyzer.Analyze(solution),
            _ => Analysis.Empty
        };
    }

    private static bool HasCompilationErrors(this Solution solution) =>
        solution.Compilation.GetDiagnostics().Any(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);
}
