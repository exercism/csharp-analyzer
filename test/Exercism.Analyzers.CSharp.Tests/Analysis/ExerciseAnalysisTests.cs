using System;
using System.Net.Http;
using System.Threading.Tasks;
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
        private readonly FakeExercismCommandLineInterface _fakeExercismCommandLineInterface;

        protected ExerciseAnalysisTests(Exercise exercise, WebApplicationFactory<Startup> factory)
        {
            _solution = new Solution(Guid.NewGuid().ToString(), exercise);
            _fakeExercismCommandLineInterface = new FakeExercismCommandLineInterface();
            _httpClient = AnalysisTestsHttpClientFactory.Create(factory, _fakeExercismCommandLineInterface);
        }

        protected async Task VerifyReturnsNoComments(string implementationFileSuffix)
        {
            var comments = await RequestAnalysis(implementationFileSuffix).ConfigureAwait(false);
            Assert.Empty(comments);
        }

        protected async Task VerifyReturnsComments(string implementationFileSuffix, params string[] expectedComments)
        {
            var comments = await RequestAnalysis(implementationFileSuffix).ConfigureAwait(false);

            foreach (var expectedComment in expectedComments)
                Assert.Contains(expectedComment, comments);
        }

        private async Task<string[]> RequestAnalysis(string implementationFileSuffix)
        {
            _fakeExercismCommandLineInterface.Configure(_solution, implementationFileSuffix);

            var response = await _httpClient.GetAsync($"/api/analyze/{_solution.Id}").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<string[]>().ConfigureAwait(false);
        }
    }
}