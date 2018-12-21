using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    public static class SolutionAnalyzer
    {
        public static async Task<ImmutableArray<Diagnostic>> Analyze(CompiledSolution compiledSolution)
        {
            var analyzers = SolutionAnalyzers.Create(compiledSolution.Solution);
            var compilationWithAnalyzers = compiledSolution.Compilation.WithAnalyzers(analyzers);
            return await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync().ConfigureAwait(false);
        }
    }
}