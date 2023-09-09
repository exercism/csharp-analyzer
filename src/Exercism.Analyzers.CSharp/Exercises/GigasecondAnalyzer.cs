using System;

namespace Exercism.Analyzers.CSharp.Exercises;

internal static class GigasecondAnalyzer
{
    public static Analysis Analyze(Solution solution)
    {
        var symbolsWithName = solution.Compilation.GetSymbolsWithName("System.DateTime");

        return new Analysis(Array.Empty<Comment>(), Array.Empty<string>());
    }
}