using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Exercism.Analyzers.CSharp.Bulk
{
    public static class BulkSolutionAnalyzer
    {
        public static async Task<BulkSolutionAnalysisRun> Run(BulkSolution testSolution)
        {
            var stopwatch = Stopwatch.StartNew();
            await CSharp.Program.Main(new[] { testSolution.Slug, testSolution.Directory });
            stopwatch.Stop();

            return CreateTestSolutionAnalyisRun(testSolution, stopwatch.Elapsed);
        }

        private static BulkSolutionAnalysisRun CreateTestSolutionAnalyisRun(BulkSolution solution, TimeSpan elapsed)
        {
            var analysisResultJsonFilePath = Path.Combine(solution.Directory, "analysis.json");
            var analysisResult = BulkSolutionAnalysisResultReader.Read(analysisResultJsonFilePath);

            return new BulkSolutionAnalysisRun(solution, analysisResult, elapsed);
        }
    }
}