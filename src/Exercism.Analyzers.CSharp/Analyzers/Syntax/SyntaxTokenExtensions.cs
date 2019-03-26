using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax
{
    internal static class SyntaxTokenExtensions
    {
        public static bool IsSafeEquivalentTo(this SyntaxToken token, SyntaxToken other) =>
            token.NormalizeWhitespace().WithoutTrivia().IsEquivalentTo(other.NormalizeWhitespace().WithoutTrivia());
    }
}