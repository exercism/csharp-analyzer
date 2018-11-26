using System;
using System.Net.Http;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Analyzers;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Exercism.Analyzers.CSharp.Tests.Analysis.Solutions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Exercism.Analyzers.CSharp.Tests.Analysis
{
    public abstract class ExerciseAnalysisTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly Solution _solution;
        private readonly HttpClient _httpClient;
        private readonly FakeSolutionDownloader _fakeSolutionDownloader;

        protected ExerciseAnalysisTests(string slug, WebApplicationFactory<Startup> factory)
        {
            _solution = new Solution(slug, Guid.NewGuid().ToString());
            _fakeSolutionDownloader = new FakeSolutionDownloader();
            _httpClient = AnalysisTestsHttpClientFactory.Create(factory, _fakeSolutionDownloader);
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

        private async Task<Diagnostic[]> RequestAnalysis(string implementationFileSuffix)
        {
            _fakeSolutionDownloader.Configure(_solution, implementationFileSuffix);

            var response = await _httpClient.GetAsync($"/api/analyze/{_solution.Slug}/{_solution.Uuid}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<Diagnostic[]>();
        }
    }
}