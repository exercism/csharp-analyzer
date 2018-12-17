using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Exercism.Analyzers.CSharp.Analysis.Compiling
{
    internal static class SyntaxTreeExtensions
    {
        public static SyntaxTree Rewrite(this SyntaxTree tree, CSharpSyntaxRewriter rewriter)
            => tree.WithRootAndOptions(rewriter.Visit(tree.GetRoot()), tree.Options);
    }
}