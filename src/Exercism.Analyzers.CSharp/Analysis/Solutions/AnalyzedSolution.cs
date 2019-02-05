namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public class AnalyzedSolution
    {
        public Solution Solution { get; }
        public SolutionStatus Status { get; }
        public string[] Comments { get; }
        
        public AnalyzedSolution(in Solution solution, SolutionStatus result, string[] comments) =>
            (Solution, Status, Comments) = (solution, result, comments);
        
        public static AnalyzedSolution CreateApproved(in Solution solution, params string[] comments) =>
            new AnalyzedSolution(solution, SolutionStatus.Approved, comments);
        
        public static AnalyzedSolution CreateRequiresChange(in Solution solution, params string[] comments) =>
            new AnalyzedSolution(solution, SolutionStatus.RequiresChange, comments);
        
        public static AnalyzedSolution CreateRequiresMentoring(in Solution solution, params string[] comments) =>
            new AnalyzedSolution(solution, SolutionStatus.RequiresMentoring, comments);
        
    }
}