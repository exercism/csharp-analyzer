using System.Text;

namespace Exercism.Analyzers.CSharp.Bulk
{
    internal static class BulkSolutionAnalysisReportGeneralInfo
    {
        private const int GeneralInfoColumnWidth = -9;
        
        public static StringBuilder AddGeneralInfo(this StringBuilder report, Options options) =>
            report
                .AddGeneralInfoHeader()
                .AppendLine($"{"Slug", GeneralInfoColumnWidth}: {options.Slug}")
                .AppendLine($"{"Directory", GeneralInfoColumnWidth}: {options.Directory}");

        private static StringBuilder AddGeneralInfoHeader(this StringBuilder report) =>
            report.AppendLine("## General info");
    }
}