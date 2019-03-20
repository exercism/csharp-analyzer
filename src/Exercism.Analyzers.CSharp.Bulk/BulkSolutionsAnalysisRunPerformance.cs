using System.Linq;

namespace Exercism.Analyzers.CSharp.Bulk
{
    internal class BulkSolutionsAnalysisRunPerformance
    {
        public double TotalInMilliseconds { get; }
        public double AverageInMilliseconds { get; }

        public BulkSolutionsAnalysisRunPerformance(BulkSolutionAnalysisRun[] analyses)
        {
            var elapsedMilliseconds = analyses.Select(analysis => analysis.Elapsed.TotalMilliseconds).DefaultIfEmpty().ToArray();
            TotalInMilliseconds = elapsedMilliseconds.Sum();
            AverageInMilliseconds = elapsedMilliseconds.Length == 0 ? 0.0 : TotalInMilliseconds / elapsedMilliseconds.Length;
        }
    }
}