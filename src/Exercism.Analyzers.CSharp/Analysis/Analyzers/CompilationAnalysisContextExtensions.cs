using Exercism.Analyzers.CSharp.Analysis.Compiling;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    internal static class CompilationAnalysisContextExtensions
    {
        public static bool SkipAnalysis(this CompilationAnalysisContext context) =>
            context.Compilation.HasErrors();
    }
}