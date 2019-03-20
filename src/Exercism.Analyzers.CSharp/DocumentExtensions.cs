using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Simplification;

namespace Exercism.Analyzers.CSharp
{
    internal static class DocumentExtensions
    {
        public static SyntaxNode GetReducedSyntaxRoot(this Document document)
        {
            var documentWithNormalizedSyntaxRoot = document.WithSyntaxRoot(document.GetNormalizedSyntaxRoot());
            var reducedDocument = Simplifier.ReduceAsync(documentWithNormalizedSyntaxRoot).GetAwaiter().GetResult();
            return reducedDocument.GetSyntaxRootAsync().GetAwaiter().GetResult();
        }

        private static SyntaxNode GetNormalizedSyntaxRoot(this Document document) =>
            document.GetSyntaxRootAsync().GetAwaiter().GetResult()
                .NormalizeWhitespace()
                .WithAdditionalAnnotations(Simplifier.Annotation);
    }
}