using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Exercism.Analyzers.CSharp.Tests
{
    public class AnalysisIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public AnalysisIntegrationTests(WebApplicationFactory<Startup> factory) => _factory = factory;

        [Theory]
        [InlineData("/api/analyze/61b523d5-54da-4df4-8baa-c5df2f52ab81")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/plain; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.Equal("value", await response.Content.ReadAsStringAsync());
        }
    }
}
