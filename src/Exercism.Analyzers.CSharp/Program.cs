using System.Linq;
using CommandLine;
using Serilog;

namespace Exercism.Analyzers.CSharp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Logging.Configure();

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunAnalysis);
        }

        private static void RunAnalysis(Options options)
        {
            Log.Information("Analyzing {Exercise} solution in directory {Directory}", options.Slug, options.Directory);

            var solution = SolutionParser.Parse(options);
            var solutionAnalysis = SolutionAnalyzer.Analyze(solution);
            SolutionAnalysisWriter.WriteToFile(options, solutionAnalysis);

            Log.Information("Analyzed {Exercise} solution in directory {Directory}. Status: {Status}. Comments: {Comments}", options.Slug, options.Directory, solutionAnalysis.Status, solutionAnalysis.Comments.Select(comment => comment.Comment));
        }

        private static Solution Parse(Options options) =>
            SolutionParser.Parse(options);

        private static SolutionAnalysis Analyze(Solution solution) =>
            SolutionAnalyzer.Analyze(solution);

        private static void WriteToFile(SolutionAnalysis solutionAnalysis, Options options) =>
            SolutionAnalysisWriter.Write(options, solutionAnalysis);
    }
}