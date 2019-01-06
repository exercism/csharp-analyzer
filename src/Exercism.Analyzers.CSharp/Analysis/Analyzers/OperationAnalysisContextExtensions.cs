using Exercism.Analyzers.CSharp.Analysis.Compiling;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    internal static class OperationAnalysisContextExtensions
    {
        public static bool SkipAnalysis(this OperationAnalysisContext context) =>
            context.Operation.Syntax.SkipAnalysis() || context.HasCompilationErrors();

        private static bool HasCompilationErrors(this OperationAnalysisContext context) =>
            context.Compilation.HasErrors();
    }
}