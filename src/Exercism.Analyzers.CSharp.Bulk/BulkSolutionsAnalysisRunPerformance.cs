using System.Linq;

namespace Exercism.Analyzers.CSharp.Bulk
{
    public class BulkSolutionsAnalysisRunPerformance
    {
        public double AverageInMilliseconds { get; }
        public double MaximumInMilliseconds { get; }
        public double MinimumInMilliseconds { get; }

        public BulkSolutionsAnalysisRunPerformance(BulkSolutionAnalysisRun[] analyses)
        {
            var elapsedMilliseconds = analyses.Select(analysis => analysis.Elapsed.TotalMilliseconds).DefaultIfEmpty().ToArray();
            AverageInMilliseconds = elapsedMilliseconds.Length == 0 ? 0.0 : elapsedMilliseconds.Sum() / elapsedMilliseconds.Length;
            MaximumInMilliseconds = elapsedMilliseconds.Max();
            MinimumInMilliseconds = elapsedMilliseconds.Min();
        }
    }
}