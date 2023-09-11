using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;

using Exercism.Analyzers.CSharp.Exercises;

using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp;

internal record Solution(string Slug, Compilation Compilation);

internal record Analysis(HashSet<Comment> Comments, HashSet<string> Tags)
{
    public static readonly Analysis Empty = new(new HashSet<Comment>(), new HashSet<string>());

    public Analysis() : this(new HashSet<Comment>(), new HashSet<string>())
    {
    }
}

internal static class Analyzer
{
    public static Analysis Analyze(Solution solution)
    {
        if (solution.HasCompilationErrors())
            return Analysis.Empty; // We can't really analyze the solution when there are compilation errors
        
        return solution.Slug switch
        {
            "leap" => LeapAnalyzer.Analyze(solution),
            "gigasecond" => GigasecondAnalyzer.Analyze(solution),
            "two-fer" => TwoFerAnalyzer.Analyze(solution),
            "weighing-machine" => WeighingMachineAnalyzer.Analyze(solution),
            _ => Analysis.Empty
        };
    }

    private static bool HasCompilationErrors(this Solution solution) =>
        solution.Compilation.GetDiagnostics().Any(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);
}
