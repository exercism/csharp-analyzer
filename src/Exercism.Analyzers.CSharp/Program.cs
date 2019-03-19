using Humanizer;

namespace Exercism.Analyzers.CSharp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Logging.Configure();

            // TODO: use library for options
            Analyze(new Options(args));
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