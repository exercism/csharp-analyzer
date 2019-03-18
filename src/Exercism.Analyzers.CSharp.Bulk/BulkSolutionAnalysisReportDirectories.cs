using System.Text;

namespace Exercism.Analyzers.CSharp.Bulk
{
    internal static class BulkSolutionAnalysisReportDirectories
    {
        public static StringBuilder AddDirectories(this StringBuilder report, BulkSolutionsAnalysisRun analysisRun, Options options)
        {
            if (!options.ListDirectories)
                return report;
            
            return report
                .AddDirectoriesHeader()
                .AddDirectoriesForStatus(analysisRun.ApprovedAsOptimal, "Approve (optimal)")
                .AddDirectoriesForStatus(analysisRun.ApprovedWithComment, "Approve (comment)")
                .AddDirectoriesForStatus(analysisRun.DisapprovedWithComment, "Disapprove (comment)")
                .AddDirectoriesForStatus(analysisRun.ReferredToMentor, "Refer to mentor");
        }

        private static StringBuilder AddDirectoriesHeader(this StringBuilder report) =>
            report.AppendLine("## Directories");

        private static StringBuilder AddDirectoriesForStatus(this StringBuilder report,
            BulkSolutionsAnalysisRunStatistics statistics, string status)
        {
            if (statistics.Directories.Length == 0)
                return report;

            report
                .AppendLine()
                .AppendLine($"### {status}");
           
            foreach (var directory in statistics.Directories)
                report.AppendLine(directory);

            return report;
        }
    }
}