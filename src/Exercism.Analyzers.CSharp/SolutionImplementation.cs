namespace Exercism.Analyzers.CSharp
{
    internal class SolutionImplementation
    {
        public Solution Solution { get; }
        public Implementation Implementation { get; }        

        public SolutionImplementation(Solution solution, Implementation implementation) =>
            (Solution, Implementation) = (solution, implementation);
        
        public SolutionAnalysis ApproveAsOptimal() =>
            ToSolutionAnalysis(SolutionStatus.ApproveAsOptimal);

        public SolutionAnalysis ApproveWithComment(params string[] comments) =>
            ToSolutionAnalysis(SolutionStatus.ApproveWithComment, comments);

        public SolutionAnalysis DisapproveWithComment(params string[] comments) =>
            ToSolutionAnalysis(SolutionStatus.DisapproveWithComment, comments);

        public SolutionAnalysis ReferToMentor(params string[] comments) =>
            ToSolutionAnalysis(SolutionStatus.ReferToMentor, comments);

        private SolutionAnalysis ToSolutionAnalysis(SolutionStatus status, params string[] comments) =>
            new SolutionAnalysis(Solution, new SolutionAnalysisResult(status, comments));

        public bool IsEquivalentTo(string expectedCode) =>
            Implementation.IsEquivalentTo(expectedCode);
    }
}