using System.Linq;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Compiling;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public class SolutionAnalyzer
    {
        private static readonly string[] CompileErrorsComments = {"The solution does not compile."};
        
        private readonly ILogger<SolutionAnalyzer> _logger;

        public SolutionAnalyzer(ILogger<SolutionAnalyzer> logger) => _logger = logger;

        public async Task<AnalyzedSolution> Analyze(CompiledSolution compiledSolution)
        {
            if (compiledSolution.Compilation.HasErrors())
                return new AnalyzedSolution(compiledSolution.Solution, SolutionStatus.RequiresChange, CompileErrorsComments);
            
            _logger.LogInformation("Retrieving diagnostics for solution {ID}", compiledSolution.Solution.Id);
            
            var diagnostics = await compiledSolution.CompilationWithAnalyzers.GetAnalyzerDiagnosticsAsync();

            _logger.LogInformation("Retrieved diagnostics for solution {ID}: {ErrorCount} errors, {WarningCount} warnings and {InformationCount} information",
                compiledSolution.Solution.Id, 
                diagnostics.Count(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error),
                diagnostics.Count(diagnostic => diagnostic.Severity == DiagnosticSeverity.Warning),
                diagnostics.Count(diagnostic => diagnostic.Severity == DiagnosticSeverity.Info));

            var comments = diagnostics.Select(diagnostic => diagnostic.GetMessage()).ToArray();
            
            _logger.LogInformation("Retrieved comments for solution {ID}: {Comments}",
                compiledSolution.Solution.Id, comments);
            
            return new AnalyzedSolution(compiledSolution.Solution, SolutionStatus.Approved, comments);
        }
    }
}