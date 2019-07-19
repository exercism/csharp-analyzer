namespace Exercism.Analyzers.CSharp
{
    internal class SolutionAnalysis
    {
        public SolutionStatus Status { get; }
        public SolutionComment[] Comments { get; }

        public SolutionAnalysis(SolutionStatus status, SolutionComment[] comments) =>
            (Status, Comments) = (status, comments);
    }
}