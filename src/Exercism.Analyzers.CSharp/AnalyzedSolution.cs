namespace Exercism.Analyzers.CSharp
{
    public class AnalyzedSolution
    {
        public Solution Solution { get; }
        public SolutionStatus Status { get; }
        public string[] Messages { get; }

        public AnalyzedSolution(Solution solution, SolutionStatus status, params string[] messages)
            => (Solution, Status, Messages) = (solution, status, messages);
    }
}