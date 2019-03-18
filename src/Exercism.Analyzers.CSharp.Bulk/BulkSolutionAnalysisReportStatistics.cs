using System.Text;

namespace Exercism.Analyzers.CSharp.Bulk
{
    internal static class BulkSolutionAnalysisReportStatistics
    {
        private const int StatisticsStatusColumnWidth = 20;
        private const int StatisticsCountColumnWidth = 5;
        private const int StatisticsCommentsColumnWidth = 8;
        private const int StatisticsAverageColumnWidth = 6;
        private const int StatisticsTotalColumnWidth = 8;
        
        public static StringBuilder AddStatistics(this StringBuilder report, BulkSolutionsAnalysisRun analysisRun) =>
            report
                .AddStatisticsHeader()
                .AddStatisticsForStatus(analysisRun.ApprovedAsOptimal, "Approve (optimal)")
                .AddStatisticsForStatus(analysisRun.ApprovedWithComment, "Approve (comment)")
                .AddStatisticsForStatus(analysisRun.DisapprovedWithComment, "Disapprove (comment)")
                .AddStatisticsForStatus(analysisRun.ReferredToMentor, "Refer to mentor")
                .AddStatisticsForStatus(analysisRun.All, "Total");

        private static StringBuilder AddStatisticsHeader(this StringBuilder report) =>
            report
                .AppendLine("## Statistics")
                .AppendLine($"| {"Status",StatisticsStatusColumnWidth} | {"Count",StatisticsCountColumnWidth} | {"Comments",StatisticsCommentsColumnWidth} | {"Avg",StatisticsAverageColumnWidth} | {"Total",StatisticsTotalColumnWidth} |")
                .AppendLine($"| {"".PadRight(StatisticsStatusColumnWidth, '-')}:| {"".PadRight(StatisticsCountColumnWidth, '-')}:| {"".PadRight(StatisticsCommentsColumnWidth, '-')}:| {"".PadRight(StatisticsAverageColumnWidth, '-')}:| {"".PadRight(StatisticsTotalColumnWidth, '-')}:|");
        
        private static StringBuilder AddStatisticsForStatus(this StringBuilder report,
            BulkSolutionsAnalysisRunStatistics statistics, string status) =>
            report.AppendLine($"| {status,StatisticsStatusColumnWidth} | {statistics.Count,StatisticsCountColumnWidth} | {statistics.CommentsWithCount.Count,StatisticsCommentsColumnWidth} | {statistics.Performance.AverageInMilliseconds,4:#0}ms | {statistics.Performance.TotalInMilliseconds,4:#0}ms | ");
    }
}