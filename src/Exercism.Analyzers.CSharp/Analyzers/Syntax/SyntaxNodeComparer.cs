using Exercism.Analyzers.CSharp.Analyzers.Syntax.Rewriting;
using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax
{
    internal static class SyntaxNodeComparer
    {
        private static readonly NormalizeSyntaxRewriter NormalizeSyntaxRewriter = new NormalizeSyntaxRewriter();

        public static bool IsEquivalentToNormalized(SyntaxNode node, SyntaxNode other) =>
            node.Normalize().IsEquivalentTo(other.Normalize());

        private static SyntaxNode Normalize(this SyntaxNode node) => NormalizeSyntaxRewriter.Visit(node);
    }
}