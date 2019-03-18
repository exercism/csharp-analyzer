using System;
using System.Linq;

namespace Exercism.Analyzers.CSharp.Bulk
{
    public class BulkSolutionsAnalysisRun
    {   
        public BulkSolutionsAnalysisRunStatistics All { get; }
        public BulkSolutionsAnalysisRunStatistics ApprovedAsOptimal { get; }
        public BulkSolutionsAnalysisRunStatistics ApprovedWithComment { get; }
        public BulkSolutionsAnalysisRunStatistics DisapprovedWithComment { get; }
        public BulkSolutionsAnalysisRunStatistics ReferredToMentor { get; }

        public BulkSolutionsAnalysisRun(BulkSolutionAnalysisRun[] analyses)
        {
            All = ToStats(analyses, _ => true);
            ApprovedAsOptimal = ToStats(analyses, IsApprovedAsOptimal);
            ApprovedWithComment = ToStats(analyses, IsApprovedWithComment);
            DisapprovedWithComment = ToStats(analyses, IsDisapprovedWithComment);
            ReferredToMentor = ToStats(analyses, IsReferredToMentor);
        }

        private static BulkSolutionsAnalysisRunStatistics ToStats(BulkSolutionAnalysisRun[] analyses,
            Predicate<BulkSolutionAnalysisRun> filter) =>
            new BulkSolutionsAnalysisRunStatistics(analyses.Where(analysis => filter(analysis)).ToArray());

        private static bool IsApprovedAsOptimal(BulkSolutionAnalysisRun analysis) =>
            analysis.AnalysisResult.Status == "approve_as_optimal";
        
        private static bool IsApprovedWithComment(BulkSolutionAnalysisRun analysis) =>
            analysis.AnalysisResult.Status == "approve_with_comment";
        
        private static bool IsDisapprovedWithComment(BulkSolutionAnalysisRun analysis) =>
            analysis.AnalysisResult.Status == "disapprove_with_comment";
        
        private static bool IsReferredToMentor(BulkSolutionAnalysisRun analysis) =>
            analysis.AnalysisResult.Status == "refer_to_mentor";
    }
}