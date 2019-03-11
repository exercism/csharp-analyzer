using System.Linq;
using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp
{
    internal class Implementation
    {
        public SyntaxNode SyntaxNode { get; }

        public Implementation(SyntaxNode syntaxNode) => SyntaxNode = syntaxNode;
        
        public bool IsEquivalentTo(string expectedCode)
        {
            var expectedSyntaxNode = SyntaxNodeParser.ParseNormalizedRoot(expectedCode);
            return SyntaxNode.IsEquivalentTo(expectedSyntaxNode);
        }
        
        public bool HasErrors() =>
            SyntaxNode.GetDiagnostics().Any(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);
    }
}