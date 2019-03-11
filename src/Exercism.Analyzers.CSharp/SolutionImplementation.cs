using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp
{
    internal class SolutionImplementation
    {
        public Solution Solution { get; }
        public SyntaxNode SyntaxNode { get; }

        public SolutionImplementation(Solution solution, SyntaxNode syntaxNode) =>
            (Solution, SyntaxNode) = (solution, syntaxNode);
        
        public bool IsEquivalentTo(string expectedCode)
        {
            var expectedSyntaxNode = SyntaxNodeParser.ParseNormalizedRoot(expectedCode);
            return SyntaxNode.IsEquivalentTo(expectedSyntaxNode);
        }
    }
}