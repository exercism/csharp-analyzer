using System.Collections.Generic;
using System.Linq;

namespace Exercism.Analyzers.CSharp.Bulk
{
    internal class BulkSolutionsAnalysisRunStatistics
    {
        public int Count { get; }
        public Dictionary<string, int> CommentsWithCount { get; }
        public BulkSolutionsAnalysisRunPerformance Performance { get; }

        public BulkSolutionsAnalysisRunStatistics(BulkSolutionAnalysisRun[] analyses)
        {
            Count = analyses.Length;
            CommentsWithCount = ToCommentsWithCount(analyses);
            Performance = new BulkSolutionsAnalysisRunPerformance(analyses);
        }

        private static Dictionary<string, int> ToCommentsWithCount(BulkSolutionAnalysisRun[] analyses) =>
            analyses
                .SelectMany(analysis => analysis.AnalysisResult.Comments)
                .GroupBy(comment => comment)
                .ToDictionary(comment => comment.Key, comments => comments.Count());
    }
}