using System;

using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp;

internal record Solution(Compilation Compilation);

internal record Analysis(Comment[] Comments, string[] Tags);

internal static class Analyzer
{
    public static Analysis Analyze(Solution solution) =>
        new(Array.Empty<Comment>(), Array.Empty<string>());
}
