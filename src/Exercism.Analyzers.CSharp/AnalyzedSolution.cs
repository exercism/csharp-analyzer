namespace Exercism.Analyzers.CSharp
{
    internal class AnalyzedSolution
    {
        public Solution Solution { get; }
        public SolutionStatus Status { get; }
        public string[] Comments { get; }

        public AnalyzedSolution(Solution solution, SolutionStatus status, params string[] comments) =>
            (Solution, Status, Comments) = (solution, status, comments);
    }
}