using System.Linq;

using Exercism.Analyzers.CSharp.Syntax.Rewriting;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Syntax.Comparison
{
    internal static class SyntaxNodeComparer
    {
        private static readonly NormalizeSyntaxRewriter NormalizeSyntaxRewriter = new NormalizeSyntaxRewriter();

        public static bool IsEquivalentToNormalized(SyntaxNode node, SyntaxNode other)
        {
            if (node == null && other == null)
                return true;

            if (node == null || other == null)
                return false;

            return node.Normalize().IsEquivalentTo(other.Normalize());
        }

        private static SyntaxNode Normalize(this SyntaxNode node) => NormalizeSyntaxRewriter.Visit(node);

        public static MemberAccessExpressionSyntax GetMethodCalled(this SyntaxNode syntaxNode, string methodName) =>
            syntaxNode
                .DescendantNodes<InvocationExpressionSyntax>()
                .Select(s => s.Expression)
                .OfType<MemberAccessExpressionSyntax>()
                .FirstOrDefault(s => s.Name.Identifier.Text == methodName);
    }
}