using System;
using System.Text;

namespace Exercism.Analyzers.CSharp.Bulk
{
    internal static class BulkSolutionAnalysisReport
    {
        public static void Output(BulkSolutionsAnalysisRun analysisRun) =>
            Console.WriteLine(Create(analysisRun));

        private static string Create(BulkSolutionsAnalysisRun analysisRun) =>
            new StringBuilder()
                .AddHeader()
                .AppendLine()
                .AddGeneralInfo(analysisRun)
                .AppendLine()
                .AddStatistics(analysisRun)
                .AppendLine()
                .AddComments(analysisRun)
                .ToString();

        private static StringBuilder AddHeader(this StringBuilder report) =>
            report.AppendLine("Bulk analysis result");
    }
}