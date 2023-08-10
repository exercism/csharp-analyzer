using System.Linq;

using Serilog;

namespace Exercism.Analyzers.CSharp
{
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
            Log.Information("Analyzing {Exercise} solution in directory {Directory}", options.Slug,
                options.InputDirectory);

            var solution = SolutionParser.Parse(options);
            var solutionAnalysis = SolutionAnalyzer.Analyze(solution);
            SolutionAnalysisWriter.WriteToFile(options, solutionAnalysis);

            Log.Information(
                "Analyzed {Exercise} solution in directory {Directory}. Status: {Status}. Comments: {Comments}",
                options.Slug, options.OutputDirectory, solutionAnalysis.Status,
                solutionAnalysis.Comments.Select(comment => comment.Comment));
        }
    }
}