using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public class SolutionCompiler
    {
        private readonly ILogger<SolutionCompiler> _logger;

        public SolutionCompiler(ILogger<SolutionCompiler> logger) => _logger = logger;

        public async Task<CompiledSolution> Compile(LoadedSolution loadedSolution, ImmutableArray<DiagnosticAnalyzer> analyzers)
        {
            _logger.LogInformation("Compiling solution {ID} using analyzers: {Analyzers}",
                loadedSolution.Solution.Id, loadedSolution.Solution.Id, GetAnalyzerNames(analyzers));
            
            var compilation = await loadedSolution.Project.GetCompilationAsync().ConfigureAwait(false);
            var compilationWithAnalyzers = compilation.WithAnalyzers(analyzers);
            
            _logger.LogInformation("Compiled solution {ID}", loadedSolution.Solution.Id);
            
            return new CompiledSolution(loadedSolution.Solution, compilationWithAnalyzers);
        }

        private static IEnumerable<string> GetAnalyzerNames(ImmutableArray<DiagnosticAnalyzer> analyzers) =>
            analyzers.Select(analyzer => analyzer.GetType().Name);
    }
}