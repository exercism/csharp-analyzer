using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax.Rewriting
{
    public class TriviaRemoverSyntaxRewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode Visit(SyntaxNode node)
        {
            if (node == null)
                return null;

            // TODO: see if this can be used globally
            var withoutTrivia = node.NormalizeWhitespace().WithoutTrivia();
            return base.Visit(withoutTrivia);
        }
    }
}