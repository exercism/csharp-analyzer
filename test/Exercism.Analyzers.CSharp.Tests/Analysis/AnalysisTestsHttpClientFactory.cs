using System.Net.Http;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Exercism.Analyzers.CSharp.Tests.Analysis.Solutions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Exercism.Analyzers.CSharp.Tests.Analysis
{
    internal static class AnalysisTestsHttpClientFactory
    {
        public static HttpClient Create(
            WebApplicationFactory<Startup> factory,
            FakeSolutionDownloader fakeSolutionDownloader)
        {
            return factory
                .WithWebHostBuilder(ConfigureWebHostBuilder)
                .CreateClient();

            void ConfigureWebHostBuilder(IWebHostBuilder builder) =>
                builder.ConfigureTestServices(ConfigureTestServices);

            void ConfigureTestServices(IServiceCollection services) =>
                services.AddSingleton<SolutionDownloader>(fakeSolutionDownloader);
        }
    }
}