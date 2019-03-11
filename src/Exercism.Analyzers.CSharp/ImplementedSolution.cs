using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp
{
    public class ImplementedSolution
    {
        public Solution Solution { get; }
        public SyntaxNode SyntaxNode { get; }

        public ImplementedSolution(Solution solution, SyntaxNode syntaxNode) =>
            (Solution, SyntaxNode) = (solution, syntaxNode);
        
        public bool IsEquivalentTo(string expectedCode)
        {
            var expectedSyntaxNode = SyntaxNodeParser.ParseNormalizedRoot(expectedCode);
            return SyntaxNode.IsEquivalentTo(expectedSyntaxNode);
        }
    }
}