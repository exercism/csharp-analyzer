using System;
using System.Diagnostics;
using System.IO;

namespace Exercism.Analyzers.CSharp.Bulk
{
    internal static class BulkSolutionAnalyzer
    {
        public static BulkSolutionAnalysisRun Run(BulkSolution solution)
        {
            var stopwatch = Stopwatch.StartNew();
            CSharp.Program.Main(new[] { solution.Slug, solution.Directory });
            stopwatch.Stop();

            return CreateTestSolutionAnalyisRun(solution, stopwatch.Elapsed);
        }

        private static BulkSolutionAnalysisRun CreateTestSolutionAnalyisRun(BulkSolution solution, TimeSpan elapsed)
        {
            var analysisResultJsonFilePath = Path.Combine(solution.Directory, "analysis.json");
            var analysisResult = BulkSolutionAnalysisResultReader.Read(analysisResultJsonFilePath);

            return new BulkSolutionAnalysisRun(solution, analysisResult, elapsed);
        }
    }
}