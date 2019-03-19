using System;

namespace Exercism.Analyzers.CSharp.Bulk
{
    internal class BulkSolutionAnalysisRun
    {
        public BulkSolution Solution { get; }
        public BulkSolutionAnalysisResult AnalysisResult { get; }
        public TimeSpan Elapsed { get; }

        public BulkSolutionAnalysisRun(BulkSolution solution, BulkSolutionAnalysisResult analysisResult, TimeSpan elapsed) =>
            (Solution, AnalysisResult, Elapsed) = (solution, analysisResult, elapsed);
    }
}