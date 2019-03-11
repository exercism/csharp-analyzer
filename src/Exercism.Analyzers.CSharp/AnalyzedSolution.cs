namespace Exercism.Analyzers.CSharp
{
    internal class AnalyzedSolution
    {
        public Solution Solution { get; }
        public SolutionStatus Status { get; }
        public string[] Messages { get; }

        public AnalyzedSolution(Solution solution, SolutionStatus status, params string[] messages) =>
            (Solution, Status, Messages) = (solution, status, messages);
    }
}