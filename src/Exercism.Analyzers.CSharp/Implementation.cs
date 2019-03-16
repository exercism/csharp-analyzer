using System.Linq;
using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp
{
    internal class Implementation
    {
        public SyntaxNode SyntaxNode { get; }
        public Document Document { get; }
        public Compilation Compilation { get; }

        public Implementation(SyntaxNode syntaxNode, Document document, Compilation compilation)
        {
            SyntaxNode = syntaxNode;
            Document = document;
            Compilation = compilation;
        }

        public bool HasErrors() => SyntaxNode.GetDiagnostics().Any(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);

        public bool IsEquivalentTo(string expectedCode)
        {
            // TODO: extract this to separate class
            var expectedSyntaxNode = SyntaxNodeParser.ParseNormalizedRoot(expectedCode);
            return SyntaxNode.IsEquivalentTo(expectedSyntaxNode);
        }
    }
}