using System.Text;

namespace Exercism.Analyzers.CSharp.Bulk
{
    internal static class BulkSolutionAnalysisReport
    {
        public static string Create(BulkSolutionsAnalysisRun analysisRun, Options options) =>
            new StringBuilder()
                .AddHeader()
                .AppendLine()
                .AddGeneralInfo(options)
                .AppendLine()
                .AddStatistics(analysisRun)
                .AppendLine()
                .AddComments(analysisRun)
                .AppendLine()
                .AddDirectories(analysisRun, options)
                .ToString();

        private static StringBuilder AddHeader(this StringBuilder report) =>
            report.AppendLine("Bulk analysis result");
    }
}