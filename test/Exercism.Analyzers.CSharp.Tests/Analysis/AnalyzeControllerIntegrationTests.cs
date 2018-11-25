using System;
using System.Net.Http;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Analyzers;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Exercism.Analyzers.CSharp.Tests.Analysis.Solutions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Exercism.Analyzers.CSharp.Tests.Analysis
{
    public class AnalyzeControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        private readonly FakeSolutionDownloader _fakeSolutionDownloader;

        public AnalyzeControllerIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            _fakeSolutionDownloader = new FakeSolutionDownloader();
            _httpClient = CreateHttpClient(factory, _fakeSolutionDownloader);
        }

        private static HttpClient CreateHttpClient(WebApplicationFactory<Startup> factory, FakeSolutionDownloader fakeSolutionDownloader) 
            => factory
                .WithWebHostBuilder(builder => builder.ConfigureTestServices(services => services.AddSingleton<SolutionDownloader>(fakeSolutionDownloader)))
                .CreateClient();

        [Fact]
        public async Task AnalyzeSolutionWithDiagnosticsReturnsNonEmptyDiagnosticsEnumerableAsJson()
        {
            _fakeSolutionDownloader.ImplementationFileName = "LeapFailingTests.cs";
            var response = await _httpClient.GetAsync($"/api/analyze/leap/{Guid.NewGuid()}");

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotEmpty(await response.Content.ReadAsAsync<Diagnostic[]>());
        }

        [Fact]
        public async Task AnalyzeSolutionWithoutDiagnosticsReturnsEmptyDiagnosticsEnumerableAsJson()
        {
            _fakeSolutionDownloader.ImplementationFileName = "LeapNoDiagnostics.cs";
            var response = await _httpClient.GetAsync($"/api/analyze/leap/{Guid.NewGuid()}");

            Assert.True(response.IsSuccessStatusCode);
            Assert.Empty(await response.Content.ReadAsAsync<Diagnostic[]>());Assert.True(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("/api/analyze/leap")]
        [InlineData("/api/analyze/aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")]
        [InlineData("/api/analyze/leap/5ee-9cb")]
        public async Task AnalyzeWithInvalidUrlReturnsFailure(string invalidUrl)
        {
            var response = await _httpClient.GetAsync(invalidUrl);

            Assert.False(response.IsSuccessStatusCode);
        }
    }
}
