using System.Threading.Tasks;

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
            var solution = new Solution(options.Slug, options.Directory);
            var solutionAnalysisResult = await SolutionAnalyzer.Analyze(solution);
            SolutionAnalysisWriter.Write(solutionAnalysisResult);
        }
    }
}