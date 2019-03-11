using Serilog;

namespace Exercism.Analyzers.CSharp
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            Logging.Configure();

            var directory = Options.GetDirectory(args);
            if (directory == null)
                return 1;

            var solution = SolutionReader.Read(directory);
            if (solution == null)
                return 1;

            var analyzedSolution = SolutionAnalyzer.Analyze(solution);
            if (analyzedSolution == null)
                return 1;

            AnalyzedSolutionWriter.Write(analyzedSolution);
            return 0;
        }
    }
}