using System;
using System.Linq;

namespace Exercism.Analyzers.CSharp.Bulk
{
    internal class BulkSolutionsAnalysisRun
    {
        public BulkSolutionsAnalysisRunStatistics All { get; }
        public BulkSolutionsAnalysisRunStatistics Approved { get; }
        public BulkSolutionsAnalysisRunStatistics Disapproved { get; }
        public BulkSolutionsAnalysisRunStatistics ReferredToMentor { get; }
        public Options Options { get; }

        public BulkSolutionsAnalysisRun(BulkSolutionAnalysisRun[] analyses, Options options)
        {
            All = ToStats(analyses, _ => true);
            ToStats(analyses, analysis => IsApproved(analysis));
            Approved = ToStats(analyses, IsApproved);
            Disapproved = ToStats(analyses, IsDisapproved);
            ReferredToMentor = ToStats(analyses, IsReferredToMentor);
            Options = options;
        }

        private static BulkSolutionsAnalysisRunStatistics ToStats(BulkSolutionAnalysisRun[] analyses,
            Predicate<BulkSolutionAnalysisRun> filter) =>
            new BulkSolutionsAnalysisRunStatistics(analyses.Where(analysis => filter(analysis)).ToArray());

        private static bool IsApproved(BulkSolutionAnalysisRun analysis) =>
            analysis.AnalysisResult.Status == "approve";

        private static bool IsDisapproved(BulkSolutionAnalysisRun analysis) =>
            analysis.AnalysisResult.Status == "disapprove";

        private static bool IsReferredToMentor(BulkSolutionAnalysisRun analysis) =>
            analysis.AnalysisResult.Status == "refer_to_mentor";
    }
}