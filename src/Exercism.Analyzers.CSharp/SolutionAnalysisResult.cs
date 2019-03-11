namespace Exercism.Analyzers.CSharp
{
    internal class SolutionAnalysisResult
    {
        public SolutionStatus Status { get; }
        public string[] Comments { get; }

        public SolutionAnalysisResult(SolutionStatus status, params string[] comments) =>
            (Status, Comments) = (status, comments);
    }
}