using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public class SolutionComments
    {
        private readonly ILogger<SolutionComments> _logger;

        public SolutionComments(ILogger<SolutionComments> logger) => _logger = logger;

        public async Task<string[]> GetForSolution(CompiledSolution compiledSolution)
        {
            _logger.LogInformation("Retrieving diagnostics for solution {ID}", compiledSolution.Solution.Id);
            
            var diagnostics = await compiledSolution.Compilation.GetAnalyzerDiagnosticsAsync();

            _logger.LogInformation("Retrieved diagnostics for solution {ID}: {ErrorCount} errors, {WarningCount} warnings and {InformationCount} information",
                compiledSolution.Solution.Id, 
                diagnostics.Count(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error),
                diagnostics.Count(diagnostic => diagnostic.Severity == DiagnosticSeverity.Warning),
                diagnostics.Count(diagnostic => diagnostic.Severity == DiagnosticSeverity.Info));

            var comments = diagnostics.Select(diagnostic => diagnostic.GetMessage()).ToArray();
            
            _logger.LogInformation("Retrieved comments for solution {ID}: {Comments}",
                compiledSolution.Solution.Id, comments);
            
            return comments;
        }
    }
}