using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax.Comparison
{
    internal static class SyntaxTokenComparer
    {
        public static bool IsEquivalentToNormalized(SyntaxToken token, SyntaxToken other) =>
            token.Normalize().IsEquivalentTo(other.Normalize());

        private static SyntaxToken Normalize(this SyntaxToken token) =>
            token.WithoutTrivia().NormalizeWhitespace();
    }
}