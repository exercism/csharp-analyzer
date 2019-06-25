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

            return CreateTestSolutionAnalysisRun(solution, stopwatch.Elapsed);
        }

        private static BulkSolutionAnalysisRun CreateTestSolutionAnalysisRun(BulkSolution solution, TimeSpan elapsed)
        {
            var analysisResultJsonFilePath = Path.Combine(solution.Directory, "analysis.json");
            var analysisResult = BulkSolutionAnalysisResultReader.Read(analysisResultJsonFilePath);

            return new BulkSolutionAnalysisRun(solution, analysisResult, elapsed);
        }
    }
}