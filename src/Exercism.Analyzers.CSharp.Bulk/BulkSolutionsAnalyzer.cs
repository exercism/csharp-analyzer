using System.Linq;

namespace Exercism.Analyzers.CSharp.Bulk
{
    internal static class BulkSolutionsAnalyzer
    {
        public static BulkSolutionsAnalysisRun Run(Options options)
        {
            var bulkSolutions = BulkSolutionsReader.ReadAll(options);
            var bulkSolutionAnalysisRuns = bulkSolutions.Select(BulkSolutionAnalyzer.Run).ToArray();
            return CreateTestSolutionAnalyisRun(bulkSolutionAnalysisRuns, options);
        }

        private static BulkSolutionsAnalysisRun CreateTestSolutionAnalyisRun(BulkSolutionAnalysisRun[] bulkSolution, Options options) =>
            new BulkSolutionsAnalysisRun(bulkSolution, options);
    }
}