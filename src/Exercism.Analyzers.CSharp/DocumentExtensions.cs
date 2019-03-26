using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Exercism.Analyzers.CSharp.Analyzers.Syntax.Rewriting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Simplification;

namespace Exercism.Analyzers.CSharp
{
    internal static class DocumentExtensions
    {
        public static SyntaxNode GetReducedSyntaxRoot(this Document document)
        {
            var documentWithNormalizedSyntaxRoot = document.WithSyntaxRoot(document.GetNormalizedSyntaxRoot());
            var reducedDocument = Simplifier.ReduceAsync(documentWithNormalizedSyntaxRoot).GetAwaiter().GetResult();
            var reducedSyntaxRoot = reducedDocument.GetSyntaxRootAsync().GetAwaiter().GetResult();

            return reducedSyntaxRoot.Rewrite();
        }

        private static SyntaxNode GetNormalizedSyntaxRoot(this Document document) =>
            document.GetSyntaxRootAsync().GetAwaiter().GetResult()
                .NormalizeWhitespace()
                .WithAdditionalAnnotations(Simplifier.Annotation);

        private static SyntaxNode Rewrite(this SyntaxNode reducedSyntaxRoot)
        {
            CSharpSyntaxRewriter[] rewriters =
            {
                new UnnecessaryParenthesesSyntaxRewriter(),
                new SimplifyFullyQualifiedNameSyntaxRewriter(),
                new UseBuiltInKeywordSyntaxRewriter(),
            };

            return rewriters.Aggregate(reducedSyntaxRoot, (acc, rewriter) => rewriter.Visit(acc));
        }
    }
}