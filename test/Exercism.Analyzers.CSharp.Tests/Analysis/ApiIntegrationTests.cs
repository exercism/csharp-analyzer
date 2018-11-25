using System.Net.Http;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Analyzers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Exercism.Analyzers.CSharp.Tests.Analysis
{
    public class ApiIntegrationTests : AnalyzeIntegrationTest
    {
        public ApiIntegrationTests(WebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task AnalyzeSolutionWithDiagnosticsReturnsNonEmptyDiagnosticsEnumerableAsJson()
        {
            var response = await RequestAnalysis("leap", "LeapFailingTests.cs");

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotEmpty(await response.Content.ReadAsAsync<Diagnostic[]>());
        }

        [Fact]
        public async Task AnalyzeSolutionWithoutDiagnosticsReturnsEmptyDiagnosticsEnumerableAsJson()
        {
            var response = await RequestAnalysis("leap", "LeapNoDiagnostics.cs");

            Assert.True(response.IsSuccessStatusCode);
            Assert.Empty(await response.Content.ReadAsAsync<Diagnostic[]>());
        }

        [Theory]
        [InlineData("/api/analyze")]
        [InlineData("/api/analyze/leap")]
        [InlineData("/api/analyze/aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")]
        [InlineData("/api/analyze/leap/5ee-9cb")]
        public async Task AnalyzeWithInvalidUrlReturnsFailure(string invalidUrl)
        {
            var response = await RequestUrl(invalidUrl);

            Assert.False(response.IsSuccessStatusCode);
        }
    }
}