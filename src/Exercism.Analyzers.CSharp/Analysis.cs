using System.Collections.Generic;
using System.Linq;

using Exercism.Analyzers.CSharp.Exercises;

using Microsoft.CodeAnalysis;

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

        var analyzer = CreateExerciseAnalyzer(solution);
        return analyzer.Analyze(solution);
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