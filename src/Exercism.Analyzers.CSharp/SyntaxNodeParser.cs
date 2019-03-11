using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Exercism.Analyzers.CSharp
{
    public static class SyntaxNodeParser
    {
        public static SyntaxNode ParseNormalizedRoot(string code) =>
            CSharpSyntaxTree.ParseText(code).GetRoot().NormalizeWhitespace();
    }
}