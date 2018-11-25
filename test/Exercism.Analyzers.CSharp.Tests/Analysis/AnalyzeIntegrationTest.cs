using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Exercism.Analyzers.CSharp.Tests.Analysis.Solutions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Exercism.Analyzers.CSharp.Tests.Analysis
{
    public abstract class AnalyzeIntegrationTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        private readonly FakeSolutionDownloader _fakeSolutionDownloader;

        protected AnalyzeIntegrationTest(WebApplicationFactory<Startup> factory)
        {
            _fakeSolutionDownloader = new FakeSolutionDownloader();
            _httpClient = CreateHttpClient(factory, _fakeSolutionDownloader);
        }

        private static HttpClient CreateHttpClient(WebApplicationFactory<Startup> factory, FakeSolutionDownloader fakeSolutionDownloader) 
            => factory
                .WithWebHostBuilder(builder => builder.ConfigureTestServices(services => services.AddSingleton<SolutionDownloader>(fakeSolutionDownloader)))
                .CreateClient();
        
        protected Task<HttpResponseMessage> RequestAnalysis(string slug, string implementationFileName)
        {
            _fakeSolutionDownloader.ImplementationFileName = implementationFileName;
            
            var uuid = Guid.NewGuid().ToString();
            var url = $"/api/analyze/{slug}/{uuid}";

            return RequestUrl(url);
        }

        protected Task<HttpResponseMessage> RequestUrl(string url) => _httpClient.GetAsync(url);
    }
}
