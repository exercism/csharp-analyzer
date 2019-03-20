using System.Text;

namespace Exercism.Analyzers.CSharp.Bulk
{
    internal static class BulkSolutionAnalysisReportGeneralInfo
    {
        private const int GeneralInfoColumnWidth = -9;
        
        public static StringBuilder AddGeneralInfo(this StringBuilder report, BulkSolutionsAnalysisRun analysisRun) =>
            report
                .AddGeneralInfoHeader()
                .AppendLine($"{"Slug", GeneralInfoColumnWidth}: {analysisRun.Options.Slug}")
                .AppendLine($"{"Directory", GeneralInfoColumnWidth}: {analysisRun.Options.Directory}");

        private static StringBuilder AddGeneralInfoHeader(this StringBuilder report) =>
            report.AppendLine("## General info");
    }
}