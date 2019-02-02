using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Analyzers;
using Exercism.Analyzers.CSharp.Analysis.Compiling;
using Exercism.Analyzers.CSharp.Analysis.Testing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
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
                return CreateRequiresChangeAnalyzedSolution(compiledSolution, CompileErrorsComments);
            
            _logger.LogInformation("Verifying solution {ID} passes all tests", compiledSolution.Solution.Id);
            
            if (await DoesNotPassAllTests(compiledSolution))
                return CreateRequiresChangeAnalyzedSolution(compiledSolution, DoesNotPassAllTestsComments);
            
            var analyzers = SolutionAnalyzers.Create(compiledSolution.Solution);
            var compilationWithAnalyzers = compiledSolution.Compilation.WithAnalyzers(analyzers);
            
            _logger.LogInformation("Retrieving diagnostics for solution {ID} using analyzers {AnalyzerNames}",
                compiledSolution.Solution.Id, GetAnalyzerNames(analyzers));

            var diagnostics = await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync();
            var diagnosticsBySeverity = diagnostics.ToLookup(diagnostic => diagnostic.Severity);

            _logger.LogInformation("Retrieved diagnostics for solution {ID}: {NumberOfErrors} errors, {NumberOfWarnings} warnings and {NumberOfInformation} information",
                compiledSolution.Solution.Id, 
                diagnosticsBySeverity[DiagnosticSeverity.Error].Count(),
                diagnosticsBySeverity[DiagnosticSeverity.Warning].Count(),
                diagnosticsBySeverity[DiagnosticSeverity.Info].Count());

            var errors = diagnosticsBySeverity[DiagnosticSeverity.Error].ToImmutableArray();
            if (errors.Any())
                return CreateRequiresChangeAnalyzedSolution(compiledSolution, GetDiagnosticMessages(errors));

            var comments = GetDiagnosticMessages(diagnostics);
            
            _logger.LogInformation("Retrieved comments for solution {ID}: {Comments}",
                compiledSolution.Solution.Id, comments);
            
            return CreateRequiresMentoringAnalyzedSolution(compiledSolution, comments);
        }

        private static string[] GetDiagnosticMessages(IEnumerable<Diagnostic> diagnostics) =>
            diagnostics.Select(GetMessage).ToArray();

        private static string GetMessage(Diagnostic diagnostic) => diagnostic.GetMessage();

        private static AnalyzedSolution CreateApprovedAnalyzedSolution(CompiledSolution compiledSolution, string[] comments) =>
            new AnalyzedSolution(compiledSolution.Solution, SolutionStatus.Approved, comments);
        
        private static AnalyzedSolution CreateRequiresMentoringAnalyzedSolution(CompiledSolution compiledSolution, string[] comments) =>
            new AnalyzedSolution(compiledSolution.Solution, SolutionStatus.RequiresMentoring, comments);
        
        private static AnalyzedSolution CreateRequiresChangeAnalyzedSolution(CompiledSolution compiledSolution, string[] comments) =>
            new AnalyzedSolution(compiledSolution.Solution, SolutionStatus.RequiresChange, comments);

        private static bool HasErrors(CompiledSolution compiledSolution)
            => compiledSolution.Compilation.HasErrors();

        private static async Task<bool> DoesNotPassAllTests(CompiledSolution compiledSolution)
        {
            var testRunSummary = await InMemoryXunitTestRunner.RunAllTests(compiledSolution.Compilation);
            return testRunSummary.Skipped > 0 || testRunSummary.Failed > 0;
        }
        
        private static IEnumerable<string> GetAnalyzerNames(ImmutableArray<DiagnosticAnalyzer> analyzers) =>
            analyzers.Select(analyzer => analyzer.GetType().Name);
    }
}