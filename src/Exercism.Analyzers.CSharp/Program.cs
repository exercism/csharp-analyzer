using CommandLine;
using Humanizer;

namespace Exercism.Analyzers.CSharp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Logging.Configure();

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Analyze);
        }

        private static void Analyze(Options options)
        {
            var solution = CreateSolution(options);
            var solutionAnalysisResult = SolutionAnalyzer.Analyze(solution);
            SolutionAnalysisWriter.Write(solutionAnalysisResult);
        }

        private static Solution CreateSolution(Options options) =>
            new Solution(options.Slug,options.Slug.Dehumanize().Pascalize(), options.Directory);
    }
}