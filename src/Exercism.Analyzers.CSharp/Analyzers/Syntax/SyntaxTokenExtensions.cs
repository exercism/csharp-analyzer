using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax
{
    internal static class SyntaxTokenExtensions
    {
        public static bool IsEquivalentWhenNormalized(this SyntaxToken token, SyntaxToken other) =>
            SyntaxTokenComparer.IsEquivalentToNormalized(token, other);
    }
}