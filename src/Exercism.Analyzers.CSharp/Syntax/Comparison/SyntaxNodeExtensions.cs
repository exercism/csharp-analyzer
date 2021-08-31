using Exercism.Analyzers.CSharp.Syntax.Rewriting;

using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp.Syntax.Comparison
{
    internal static class SyntaxNodeExtensions
    {
        public static SyntaxNode Simplify(this SyntaxNode node) =>
            SyntaxNodeSimplifier.Simplify(node);

        public static bool IsEquivalentWhenNormalized(this SyntaxNode node, SyntaxNode other) =>
            SyntaxNodeComparer.IsEquivalentToNormalized(node, other);
    }
}