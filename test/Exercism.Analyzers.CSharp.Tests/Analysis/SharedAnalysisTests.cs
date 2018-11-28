using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Analyzers.Rules;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Exercism.Analyzers.CSharp.Tests.Analysis
{
    public class SharedAnalysisTests : ExerciseAnalysisTests
    {
        private const string Slug = "shared";

        public SharedAnalysisTests(WebApplicationFactory<Startup> factory) : base(Slug, factory)
        {
        }

        [Fact]
        public async Task AnalyzeSolutionWithCompileErrorsReturnsSingleErrorDiagnosticAsJson()
        {
            var diagnostics = await RequestAnalysis("DoesNotCompile");

            Assert.Single(diagnostics, new Diagnostic("The code does not compile", DiagnosticLevel.Error));
        }
        
        [Fact]
        public async Task AnalyzeSolutionWithFailingTestsReturnsSingleErrorDiagnosticAsJson()
        {
            var diagnostics = await RequestAnalysis("FailingTests");

            Assert.Single(diagnostics, new Diagnostic("Not all tests pass", DiagnosticLevel.Error));
        }
        
        [Fact]
        public async Task AnalyzeSolutionWithNoDiagnosticsReturnsNoDiagnosticAsJson()
        {
            var diagnostics = await RequestAnalysis("NoDiagnostics");

            Assert.Empty(diagnostics);
        }

        [Fact]
        public async Task AnalyzeSolutionWithMethodThatCanBeConvertedToExpressionBodiedMemberReturnsSingleInformationDiagnosticAsJson()
        {
            var diagnostics = await RequestAnalysis("ConvertToExpressionBodiedMember");

            Assert.Single(diagnostics, new Diagnostic("Method 'IsEven' can be rewritten as an expression-bodied member.", DiagnosticLevel.Information));
        }
    }
}