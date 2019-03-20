using CommandLine;

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
            var solutionAnalysisResult = SolutionAnalyzer.Analyze(options);
            SolutionAnalysisWriter.Write(solutionAnalysisResult);
        }
    }
}