namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public class AnalyzedSolution
    {
        public Solution Solution { get; }
        public SolutionStatus Status { get; }
        public string[] Comments { get; }
        
        public AnalyzedSolution(Solution solution, SolutionStatus result, string[] comments) =>
            (Solution, Status, Comments) = (solution, result, comments);
    }
}