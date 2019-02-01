using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Compiling;
using Exercism.Analyzers.CSharp.Analysis.Testing;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public class SolutionAnalyzer
    {
        private static readonly string[] CompileErrorsComments = {"The solution does not compile."};
        private static readonly string[] DoesNotPassAllTestsComments = {"The solution does not pass all tests."};
        
        private readonly ILogger<SolutionAnalyzer> _logger;

        public SolutionAnalyzer(ILogger<SolutionAnalyzer> logger) => _logger = logger;

        public async Task<AnalyzedSolution> Analyze(CompiledSolution compiledSolution)
        {
            _logger.LogInformation("Checking solution {ID} for compile errors", compiledSolution.Solution.Id);
            
            if (HasErrors(compiledSolution))
                return new AnalyzedSolution(compiledSolution.Solution, SolutionStatus.RequiresChange, CompileErrorsComments);
            
            _logger.LogInformation("Verifying solution {ID} passes all tests", compiledSolution.Solution.Id);
            
            if (await DoesNotPassAllTests(compiledSolution))
                return new AnalyzedSolution(compiledSolution.Solution, SolutionStatus.RequiresChange, DoesNotPassAllTestsComments);
            
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

        private static bool HasErrors(CompiledSolution compiledSolution)
            => compiledSolution.Compilation.HasErrors();

        private static async Task<bool> DoesNotPassAllTests(CompiledSolution compiledSolution)
        {
            var testRunSummary = await InMemoryXunitTestRunner.RunAllTests(compiledSolution.Compilation);
            return testRunSummary.Skipped > 0 || testRunSummary.Failed > 0;
        }
    }
}