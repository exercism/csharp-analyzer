namespace Exercism.Analyzers.CSharp
{
    internal class SolutionAnalysisResult
    {
        public SolutionStatus Status { get; }
        public SolutionComment[] Comments { get; }

        public SolutionAnalysisResult(SolutionStatus status, SolutionComment[] comments) =>
            (Status, Comments) = (status, comments);
    }
}