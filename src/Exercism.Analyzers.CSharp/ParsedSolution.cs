using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp
{
    internal class ParsedSolution
    {
        public Solution Solution { get; }

        public SyntaxNode SyntaxRoot { get; }

        public ParsedSolution(Solution solution, SyntaxNode syntaxRoot) =>
            (Solution, SyntaxRoot) = (solution, syntaxRoot);

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

        public bool IsEquivalentTo(string expectedCode)
        {
            // TODO: consider doing string comparison (syntaxroot is already normalised)
            var expectedSyntaxNode = SyntaxNodeParser.ParseNormalizedRoot(expectedCode);
            return SyntaxRoot.IsEquivalentTo(expectedSyntaxNode);
        }
    }
}