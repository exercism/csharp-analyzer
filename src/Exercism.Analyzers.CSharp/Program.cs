using System;
using System.Threading.Tasks;

namespace Exercism.Analyzers.CSharp;

internal record Options(string Slug, string InputDirectory, string OutputDirectory);

public static class Program
{
    public static async Task Main(string[] args)
    {
        var options = new Options(args[0], args[1], args[2]);

        await RunAnalysis(options);
    }

    private static async Task RunAnalysis(Options options)
    {
        Console.WriteLine($"Analyzing {options.Slug} solution in directory {options.InputDirectory}");

        var solution = await Loader.Load(options);
        var analysis = Analyzer.Analyze(solution);
        Output.WriteToFile(options, analysis);

        Console.WriteLine($"Analyzed {options.Slug} solution in directory {options.InputDirectory}");
    }
}