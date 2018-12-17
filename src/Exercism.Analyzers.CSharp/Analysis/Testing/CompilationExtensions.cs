using Exercism.Analyzers.CSharp.Analysis.Compiling;
using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp.Analysis.Testing
{
    internal static class CompilationExtensions
    {
        private static readonly RemoveSkipAttributeArgumentSyntaxRewriter RemoveSkipAttributeArgumentSyntaxRewriter = new RemoveSkipAttributeArgumentSyntaxRewriter();
        
        public static Compilation EnableAllTests(this Compilation compilation)
            => compilation.Rewrite(RemoveSkipAttributeArgumentSyntaxRewriter);
    }
}