using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp
{
    // TODO: create separate class for implementation
    
    internal class SolutionImplementation
    {
        public Solution Solution { get; }
        public SyntaxNode SyntaxNode { get; }

        public SolutionImplementation(Solution solution, SyntaxNode syntaxNode) =>
            (Solution, SyntaxNode) = (solution, syntaxNode);
        
        public AnalyzedSolution ApproveAsOptimal() =>
            new AnalyzedSolution(Solution, SolutionStatus.ApproveAsOptimal);

        public AnalyzedSolution ApproveWithComment(params string[] comments) =>
            new AnalyzedSolution(Solution, SolutionStatus.ApproveWithComment, comments);

        public AnalyzedSolution DisapproveWithComment(params string[] comments) =>
            new AnalyzedSolution(Solution, SolutionStatus.DisapproveWithComment, comments);

        public AnalyzedSolution ReferToMentor(params string[] comments) =>
            new AnalyzedSolution(Solution, SolutionStatus.ReferToMentor, comments);
        
        public bool IsEquivalentTo(string expectedCode)
        {
            var expectedSyntaxNode = SyntaxNodeParser.ParseNormalizedRoot(expectedCode);
            return SyntaxNode.IsEquivalentTo(expectedSyntaxNode);
        }
    }
}