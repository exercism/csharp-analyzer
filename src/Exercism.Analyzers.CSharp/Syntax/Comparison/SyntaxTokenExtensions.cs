using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp.Syntax.Comparison
{
    internal static class SyntaxTokenExtensions
    {
        public static bool IsEquivalentWhenNormalized(this SyntaxToken token, SyntaxToken other) =>
            SyntaxTokenComparer.IsEquivalentToNormalized(token, other);
    }
}