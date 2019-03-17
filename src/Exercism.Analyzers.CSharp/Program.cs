using System.Threading.Tasks;
using Humanizer;

namespace Exercism.Analyzers.CSharp
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Logging.Configure();

            await Analyze(new Options(args));
        }

        private static async Task Analyze(Options options)
        {
            var solution = CreateSolution(options);
            var solutionAnalysisResult = await SolutionAnalyzer.Analyze(solution);
            SolutionAnalysisWriter.Write(solutionAnalysisResult);
        }

        private static Solution CreateSolution(Options options) =>
            new Solution(options.Slug,options.Slug.Dehumanize().Pascalize(), options.Directory);
    }
}