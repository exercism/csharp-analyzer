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
        private readonly ILogger<SolutionAnalyzer> _logger;

        public SolutionAnalyzer(ILogger<SolutionAnalyzer> logger) => _logger = logger;

        public async Task<AnalyzedSolution> Analyze(CompiledSolution compiledSolution)
        {
            var analyzedSolution = await CreateAnalyzedSolution(compiledSolution);

            _logger.LogInformation("Analysis result for solution {ID}: status {SolutionStatus}, comments {Comments}", 
                compiledSolution.Solution.Id, analyzedSolution.Status, analyzedSolution.Comments);

            return analyzedSolution;
        }

        private async Task<AnalyzedSolution> CreateAnalyzedSolution(CompiledSolution compiledSolution)
        {
            if (HasCompilationErrors(compiledSolution))
                return CreateAnalyzedSolutionForCompilationErrors(compiledSolution);

            if (await HasFailingTests(compiledSolution))
                return CreateAnalyzedSolutionForFailingTests(compiledSolution);

            return await CreateAnalyzedSolutionForCorrectSolution(compiledSolution);
        }

        private static bool HasCompilationErrors(CompiledSolution compiledSolution) =>
            compiledSolution.Compilation.HasErrors();

        private static AnalyzedSolution CreateAnalyzedSolutionForCompilationErrors(CompiledSolution compiledSolution) =>
            AnalyzedSolution.CreateRequiresChange(compiledSolution.Solution, "The solution does not compile.");

        private static async Task<bool> HasFailingTests(CompiledSolution compiledSolution)
        {
            var testRunSummary = await InMemoryXunitTestRunner.RunAllTests(compiledSolution.Compilation);
            return testRunSummary.Failed > 0;
        }

        private static AnalyzedSolution CreateAnalyzedSolutionForFailingTests(CompiledSolution compiledSolution) =>
            AnalyzedSolution.CreateRequiresChange(compiledSolution.Solution, "The solution does not pass all tests.");

        private async Task<AnalyzedSolution> CreateAnalyzedSolutionForCorrectSolution(CompiledSolution compiledSolution)
        {
            var diagnostics = await GetDiagnostics(compiledSolution);
            var diagnosticsBySeverity = diagnostics.ToLookup(diagnostic => diagnostic.Severity);

            _logger.LogInformation(
                "Retrieved diagnostics for solution {ID}: {NumberOfErrors} errors, {NumberOfWarnings} warnings and {NumberOfInformation} information",
                compiledSolution.Solution.Id,
                diagnosticsBySeverity[DiagnosticSeverity.Error].Count(),
                diagnosticsBySeverity[DiagnosticSeverity.Warning].Count(),
                diagnosticsBySeverity[DiagnosticSeverity.Info].Count());

            var errors = diagnosticsBySeverity[DiagnosticSeverity.Error].ToImmutableArray();
            if (errors.Any())
                return AnalyzedSolution.CreateRequiresChange(compiledSolution.Solution, GetDiagnosticMessages(errors));

            var comments = GetDiagnosticMessages(diagnostics);

            if (await CanBeApproved(compiledSolution))
                return AnalyzedSolution.CreateApproved(compiledSolution.Solution, comments);

            return AnalyzedSolution.CreateRequiresMentoring(compiledSolution.Solution, comments);
        }

        private async Task<ImmutableArray<Diagnostic>> GetDiagnostics(CompiledSolution compiledSolution)
        {
            var analyzers = AnalyzerFactory.Create(compiledSolution.Solution);

            _logger.LogInformation("Retrieving diagnostics for solution {ID} using analyzers {Analyzers}",
                compiledSolution.Solution.Id, GetAnalyzerNames(analyzers));

            var compilationWithAnalyzers = compiledSolution.Compilation.WithAnalyzers(analyzers);
            return await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync();
        }

        private async Task<bool> CanBeApproved(CompiledSolution compiledSolution)
        {
            _logger.LogInformation("Checking if solution {ID} can be automatically approved",
                compiledSolution.Solution.Id);
            
            var approvalAnalyzer = AnalyzerFactory.CreateForApproval(compiledSolution.Solution);
            if (!await approvalAnalyzer.CanBeApproved(compiledSolution.Compilation))
                return false;

            _logger.LogInformation("Solution {ID} can be automatically approved",
                compiledSolution.Solution.Id);

            return true;
        }

        private static string[] GetDiagnosticMessages(IEnumerable<Diagnostic> diagnostics) =>
            diagnostics.Select(diagnostic => diagnostic.GetMessage()).ToArray();

        private static IEnumerable<string> GetAnalyzerNames(ImmutableArray<DiagnosticAnalyzer> analyzers) =>
            analyzers.Select(analyzer => analyzer.GetType().Name);
    }
}