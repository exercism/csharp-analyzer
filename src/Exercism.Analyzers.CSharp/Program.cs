using System;

namespace Exercism.Analyzers.CSharp;

internal record Options(string Slug, string InputDirectory, string OutputDirectory);

public static class Program
{
    public static void Main(string[] args)
    {
        var options = new Options(args[0], args[1], args[2]);

        RunAnalysis(options);
    }

    private static void RunAnalysis(Options options)
    {
        Console.WriteLine($"Analyzing {options.Slug} solution in directory {options.InputDirectory}");

        var solution = SolutionParser.Parse(options);
        var solutionAnalysis = SolutionAnalyzer.Analyze(solution);
        SolutionAnalysisWriter.WriteToFile(options, solutionAnalysis);

        Console.WriteLine($"Analyzed {options.Slug} solution in directory {options.InputDirectory}");
    }
}