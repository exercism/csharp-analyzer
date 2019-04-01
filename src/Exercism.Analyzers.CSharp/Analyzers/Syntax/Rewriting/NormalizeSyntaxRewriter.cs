using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax.Rewriting
{
    public class NormalizeSyntaxRewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode Visit(SyntaxNode node)
        {
            if (node == null)
                return null;

            var normalizedNode = node.WithoutTrivia().NormalizeWhitespace();
            return base.Visit(normalizedNode);
        }
    }
}