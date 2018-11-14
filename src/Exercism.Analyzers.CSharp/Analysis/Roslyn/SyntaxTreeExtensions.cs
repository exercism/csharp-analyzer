using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Exercism.Analyzers.CSharp.Analysis.Roslyn
{
    internal static class SyntaxTreeExtensions
    {
        public static SyntaxTree WithRewrittenRoot(this SyntaxTree tree, CSharpSyntaxRewriter rewriter)
            => tree.WithRootAndOptions(rewriter.Visit(tree.GetRoot()), tree.Options);
    }
}