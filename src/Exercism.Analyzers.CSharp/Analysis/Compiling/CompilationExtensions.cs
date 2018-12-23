using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Exercism.Analyzers.CSharp.Analysis.Compiling
{
    internal static class CompilationExtensions
    {
        public static Assembly GetAssembly(this Compilation compilation)
        {
            using (var memoryStream = new MemoryStream())
            {
                compilation.Emit(memoryStream);
                return Assembly.Load(memoryStream.ToArray());    
            }
        }
        
        public static Compilation Rewrite(this Compilation compilation, CSharpSyntaxRewriter rewriter)
        {
            Compilation Rewrite(Compilation rewrittenCompilation, SyntaxTree tree) =>
                rewrittenCompilation.ReplaceSyntaxTree(tree, tree.Rewrite(rewriter));

            return compilation.SyntaxTrees.Aggregate(compilation, Rewrite);
        }
    }
}