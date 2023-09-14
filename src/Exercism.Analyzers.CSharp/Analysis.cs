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
    public static Analysis Empty => new(new HashSet<Comment>(), new HashSet<string>());
}

internal static class Analyzer
{
    public static Analysis Analyze(Solution solution)
    {
        if (solution.HasCompilationErrors())
            return Analysis.Empty; // We can't really analyze the solution when there are compilation errors

        var analyzer = CreateExerciseAnalyzer(solution);
        analyzer.Analyze(solution);
        return analyzer.Analysis;
    }

    private static bool HasCompilationErrors(this Solution solution) =>
        solution.Compilation.GetDiagnostics().Any(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);
    
    private static ExerciseAnalyzer CreateExerciseAnalyzer(Solution solution) =>
        solution.Slug switch
        {
            "leap" => new LeapAnalyzer(),
            "gigasecond" => new GigasecondAnalyzer(),
            "two-fer" => new TwoFerAnalyzer(),
            "weighing-machine" => new WeighingMachineAnalyzer(),
            _ => new ExerciseAnalyzer()
        };
}
