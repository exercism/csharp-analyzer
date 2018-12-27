using System;
using System.Net.Http;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Helpers
{
    public abstract class SolutionAnalysisTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        private readonly FakeExercise _fakeExercise;
        private readonly FakeExercismCommandLineInterface _fakeExercismCommandLineInterface;

        protected SolutionAnalysisTests(WebApplicationFactory<Startup> factory, FakeExercise fakeExercise)
        {
            _fakeExercise = fakeExercise;
            _fakeExercismCommandLineInterface = new FakeExercismCommandLineInterface();
            _httpClient = AnalysisTestsHttpClientFactory.Create(factory, _fakeExercismCommandLineInterface);
        }

        protected async Task AnalysisReturnsNoComments(string implementationFile)
        {
            var comments = await GetCommentsFromAnalysis(implementationFile).ConfigureAwait(false);
            Assert.Empty(comments);
        }

        protected async Task AnalysisReturnsComments(string implementationFile, params string[] expectedComments)
        {
            var comments = await GetCommentsFromAnalysis(implementationFile).ConfigureAwait(false);

            foreach (var expectedComment in expectedComments)
                Assert.Contains(expectedComment, comments);
        }

        private async Task<string[]> GetCommentsFromAnalysis(string implementationFile)
        {
            var fakeSolution = CreateFakeSolution(implementationFile);
            _fakeExercismCommandLineInterface.Configure(fakeSolution);

            var response = await RequestAnalysis(fakeSolution).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<string[]>().ConfigureAwait(false);
        }

        private FakeSolution CreateFakeSolution(string implementationFile) =>
            new FakeSolution(implementationFile, _fakeExercise);

        private Task<HttpResponseMessage> RequestAnalysis(FakeSolution fakeSolution)
        {
            var fakeSolutionUrl = $"/api/analyze/{fakeSolution.Id}";
            return _httpClient.GetAsync(fakeSolutionUrl);
        }
    }
}