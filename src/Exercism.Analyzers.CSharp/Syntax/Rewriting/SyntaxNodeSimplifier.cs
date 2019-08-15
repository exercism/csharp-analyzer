using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Exercism.Analyzers.CSharp.Syntax.Rewriting
{
    internal static class SyntaxNodeSimplifier
    {
        private static readonly CSharpSyntaxRewriter[] SyntaxRewriters =
        {
            new RemoveOptionalParenthesesSyntaxRewriter(),
            new SimplifyFullyQualifiedNameSyntaxRewriter(),
            new UseBuiltInKeywordSyntaxRewriter(),
            new InvertNegativeConditionalSyntaxRewriter(),
            new AddBracesSyntaxRewriter(),
            new ExponentNotationSyntaxRewriter()
        };

        public static SyntaxNode Simplify(SyntaxNode reducedSyntaxRoot) =>
            SyntaxRewriters.Aggregate(reducedSyntaxRoot, (acc, rewriter) => rewriter.Visit(acc));
    }
}