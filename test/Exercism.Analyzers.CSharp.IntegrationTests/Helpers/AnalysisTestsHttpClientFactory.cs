using System.Net.Http;
using Exercism.Analyzers.CSharp.Analysis.CommandLine;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Helpers
{
    internal static class AnalysisTestsHttpClientFactory
    {
        public static HttpClient Create(
            WebApplicationFactory<Startup> factory,
            FakeExercismCommandLineInterface fakeExercismCommandLineInterface)
        {
            return factory
                .WithWebHostBuilder(ConfigureWebHostBuilder)
                .CreateClient();

            void ConfigureWebHostBuilder(IWebHostBuilder builder) =>
                builder.ConfigureTestServices(ConfigureTestServices);

            void ConfigureTestServices(IServiceCollection services) =>
                services.AddSingleton<ExercismCommandLineInterface>(fakeExercismCommandLineInterface);
        }
    }
}