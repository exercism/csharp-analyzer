using Exercism.Analyzers.CSharp.Analysis.Solutions;

namespace Exercism.Analyzers.CSharp.Analysis
{
    public class AnalysisResult
    {
        public SolutionStatus Status { get; }
        public string[] Comments { get; }

        public AnalysisResult(SolutionStatus status, string[] comments) => (Status, Comments) = (status, comments);
    }
}