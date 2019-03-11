using Serilog;

namespace Exercism.Analyzers.CSharp
{   
    public static class Analyzer
    {
        public static int Analyze(string directory)
        {
            Log.Information("Analysing solution in {Directory}", directory);
            
            var solution = SolutionReader.Read(directory);
            if (solution.Track != Tracks.CSharp)
                return 1;

            var analysisResult = SolutionAnalyzer.Analyze(solution);
            if (analysisResult == null)
                return 0;

            AnalyzedSolutionWriter.Write(analysisResult);
            return 0;
        }
    }
}
