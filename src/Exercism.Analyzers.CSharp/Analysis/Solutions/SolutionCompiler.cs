using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public static class SolutionCompiler
    {
        public static async Task<CompiledSolution>
            Compile(LoadedSolution loadedSolution, ImmutableArray<DiagnosticAnalyzer> analyzers)
        {
            var compilation = await loadedSolution.Project.GetCompilationAsync().ConfigureAwait(false);
            var compilationWithAnalyzers = compilation.WithAnalyzers(analyzers);
            return new CompiledSolution(loadedSolution.Solution, compilationWithAnalyzers);
        }
    }
}