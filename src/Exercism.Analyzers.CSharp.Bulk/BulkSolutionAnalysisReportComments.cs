using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exercism.Analyzers.CSharp.Bulk
{
    internal static class BulkSolutionAnalysisReportComments
    {
        private const int CommentsCountColumnWidth = 5;

        public static StringBuilder AddComments(this StringBuilder report, BulkSolutionsAnalysisRun analysisRun) =>
            report
                .AddCommentsHeader()
                .AddCommentsForStatus(analysisRun.ApprovedWithComment, "Approve (comment)")
                .AddCommentsForStatus(analysisRun.DisapprovedWithComment, "Disapprove (comment)")
                .AddCommentsForStatus(analysisRun.All, "Total");

        private static StringBuilder AddCommentsHeader(this StringBuilder report) =>
            report.AppendLine("## Comments");

        private static StringBuilder AddCommentsForStatus(this StringBuilder report,
            BulkSolutionsAnalysisRunStatistics statistics, string status)
        {
            if (statistics.CommentsWithCount.Count == 0)
                return report;

            var maxCommentLength = statistics.CommentsWithCount.Keys.Max(comment => comment.Length);

            report
                .AppendLine()
                .AddCommentsForStatusHeader(status)
                .AppendLine($"| {"Comment".PadRight(maxCommentLength, ' ')} | {"Count".PadRight(CommentsCountColumnWidth, ' ')} |")
                .AppendLine($"| {"".PadRight(maxCommentLength, '-')}:| {"".PadRight(CommentsCountColumnWidth, '-')}:|");

            foreach (var commentWithCount in statistics.CommentsWithCount)
                report.AddCommentCount(commentWithCount, maxCommentLength);

            return report;
        }

        private static StringBuilder AddCommentsForStatusHeader(this StringBuilder report, string status) =>
            report.AppendLine($"### {status}");

        private static void AddCommentCount(this StringBuilder report, KeyValuePair<string, int> commentWithCount,
            int maxCommentLength) =>
            report.AppendLine($"| {commentWithCount.Key.PadRight(maxCommentLength, ' ')} | {commentWithCount.Value,CommentsCountColumnWidth} |");
    }
}