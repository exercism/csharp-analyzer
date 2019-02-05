using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis;
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

        protected async Task ApprovedWithoutComments([CallerMemberName]string testMethodName = "") =>
            await AnalysisResultWithoutComments(SolutionStatus.Approved, testMethodName);

        protected async Task ApprovedWithComment(string expected, [CallerMemberName]string testMethodName = "") =>
            await AnalysisResultWithComment(SolutionStatus.Approved, expected, testMethodName);

        protected async Task RequiresMentoringWithoutComments([CallerMemberName]string testMethodName = "") =>
            await AnalysisResultWithoutComments(SolutionStatus.RequiresMentoring, testMethodName);

        protected async Task RequiresMentoringWithComment(string expected, [CallerMemberName]string testMethodName = "") =>
            await AnalysisResultWithComment(SolutionStatus.RequiresMentoring, expected, testMethodName);

        protected async Task RequiresChangeWithComment(string expected, [CallerMemberName]string testMethodName = "") =>
            await AnalysisResultWithComment(SolutionStatus.RequiresChange, expected, testMethodName);

        protected async Task RequiresChangeWithSingleComment(string expected, [CallerMemberName]string testMethodName = "") =>
            await AnalysisResultWithSingleComment(SolutionStatus.RequiresChange, expected, testMethodName);

        private async Task AnalysisResultWithoutComments(SolutionStatus status, string testMethodName)
        {
            var analysisResult = await GetAnalysisResult(testMethodName);
            Assert.Equal(status, analysisResult.Status);
            Assert.Empty(analysisResult.Comments);
        }

        private async Task AnalysisResultWithComment(SolutionStatus status, string expected, string testMethodName)
        {
            var analysisResult = await GetAnalysisResult(testMethodName);
            Assert.Equal(status, analysisResult.Status);
            Assert.Contains(expected, analysisResult.Comments);
        }

        private async Task AnalysisResultWithSingleComment(SolutionStatus status, string expected, string testMethodName)
        {
            var analysisResult = await GetAnalysisResult(testMethodName);
            Assert.Equal(status, analysisResult.Status);
            Assert.Single(analysisResult.Comments, expected);
        }

        private async Task<AnalysisResult> GetAnalysisResult([CallerMemberName]string testMethodName = "")
        {
            var fakeSolution = CreateFakeSolution(testMethodName);
            _fakeExercismCommandLineInterface.Configure(fakeSolution);

            var response = await RequestAnalysis(fakeSolution);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<AnalysisResult>();
        }

        private FakeSolution CreateFakeSolution(string testMethodName) =>
            new FakeSolution(TestMethodNameToImplementationFile(testMethodName), _fakeExercise, SolutionCategory);

        private async Task<HttpResponseMessage> RequestAnalysis(FakeSolution fakeSolution)
        {
            var fakeSolutionUrl = $"/api/analyze/{fakeSolution.Id}";
            return await _httpClient.GetAsync(fakeSolutionUrl);
        }

        private static string TestMethodNameToImplementationFile(string testMethodName) =>
            testMethodName
                .Replace("Approved", string.Empty)
                .Replace("RequiresMentoring", string.Empty)
                .Replace("RequiresChange", string.Empty)
                .Replace("WithComment", string.Empty)
                .Replace("WithSingleComment", string.Empty)
                .Replace("WithoutComments", string.Empty);
        
        private string SolutionCategory => GetType().Name.Replace("Tests", "");
    }
}