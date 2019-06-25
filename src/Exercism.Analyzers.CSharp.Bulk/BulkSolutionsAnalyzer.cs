using System.Linq;

namespace Exercism.Analyzers.CSharp.Bulk
{
    internal static class BulkSolutionsAnalyzer
    {
        public static BulkSolutionsAnalysisRun Run(Options options)
        {
            var bulkSolutions = BulkSolutionsReader.ReadAll(options);
            var bulkSolutionAnalysisRuns = bulkSolutions.Select(BulkSolutionAnalyzer.Run).ToArray();
            return CreateTestSolutionAnalysisRun(bulkSolutionAnalysisRuns, options);
        }

        private static BulkSolutionsAnalysisRun CreateTestSolutionAnalysisRun(BulkSolutionAnalysisRun[] bulkSolution, Options options) =>
            new BulkSolutionsAnalysisRun(bulkSolution, options);
    }
}