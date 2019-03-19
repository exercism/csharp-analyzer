using System.Collections.Generic;

namespace Exercism.Analyzers.CSharp.Bulk
{
    public static class BulkSolutionsAnalyzer
    {
        public static BulkSolutionsAnalysisRun Run(IEnumerable<BulkSolution> bulkSolutions)
        {
            var bulkSolutionAnalysisRuns = new List<BulkSolutionAnalysisRun>();

            // We perform a sequential analysis, not parallel
            foreach (var bulkSolution in bulkSolutions)
                bulkSolutionAnalysisRuns.Add(BulkSolutionAnalyzer.Run(bulkSolution));

            return CreateTestSolutionAnalyisRun(bulkSolutionAnalysisRuns.ToArray());
        }

        private static BulkSolutionsAnalysisRun CreateTestSolutionAnalyisRun(BulkSolutionAnalysisRun[] bulkSolution) =>
            new BulkSolutionsAnalysisRun(bulkSolution);
    }
}