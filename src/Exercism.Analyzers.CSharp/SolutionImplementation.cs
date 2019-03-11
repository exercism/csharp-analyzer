namespace Exercism.Analyzers.CSharp
{
    internal class SolutionImplementation
    {
        public Solution Solution { get; }
        public Implementation Implementation { get; }        

        public SolutionImplementation(Solution solution, Implementation implementation) =>
            (Solution, Implementation) = (solution, implementation);
        
        public AnalyzedSolution ApproveAsOptimal() =>
            new AnalyzedSolution(Solution, SolutionStatus.ApproveAsOptimal);

        public AnalyzedSolution ApproveWithComment(params string[] comments) =>
            new AnalyzedSolution(Solution, SolutionStatus.ApproveWithComment, comments);

        public AnalyzedSolution DisapproveWithComment(params string[] comments) =>
            new AnalyzedSolution(Solution, SolutionStatus.DisapproveWithComment, comments);

        public AnalyzedSolution ReferToMentor(params string[] comments) =>
            new AnalyzedSolution(Solution, SolutionStatus.ReferToMentor, comments);

        public bool IsEquivalentTo(string expectedCode) =>
            Implementation.IsEquivalentTo(expectedCode);
    }
}