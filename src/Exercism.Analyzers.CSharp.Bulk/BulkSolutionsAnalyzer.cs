using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exercism.Analyzers.CSharp.Bulk
{
    public static class BulkSolutionsAnalyzer
    {
        public static async Task<BulkSolutionsAnalysisRun> Run(IEnumerable<BulkSolution> bulkSolutions)
        {
            var bulkSolutionAnalysisRuns = new List<BulkSolutionAnalysisRun>();

            // We perform a sequential analysis, not parallel
            foreach (var bulkSolution in bulkSolutions)
                bulkSolutionAnalysisRuns.Add(await BulkSolutionAnalyzer.Run(bulkSolution));

            return CreateTestSolutionAnalyisRun(bulkSolutionAnalysisRuns.ToArray());
        }

        private static BulkSolutionsAnalysisRun CreateTestSolutionAnalyisRun(BulkSolutionAnalysisRun[] bulkSolution) =>
            new BulkSolutionsAnalysisRun(bulkSolution);
    }
}