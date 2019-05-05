using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Syntax.Rewriting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax
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