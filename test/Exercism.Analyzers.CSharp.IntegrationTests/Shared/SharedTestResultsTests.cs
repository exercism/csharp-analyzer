using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Shared
{
    public class SharedTestResultsTests : SolutionAnalysisTests
    {
        public SharedTestResultsTests(WebApplicationFactory<Startup> factory) : base(factory, FakeExercise.Shared)
        {
        }

        [Fact]
        public Task PassingAllTestsDoesNotReturnComments() =>
            AnalysisReturnsNoComments("PassesAllTests");

        [Fact]
        public Task FailsSingleTestReturnsComment() =>
            AnalysisReturnsComments("FailsSingleTest", "The solution does not pass all tests.");

        [Fact]
        public Task FailsMultipleTestsReturnsComment() =>
            AnalysisReturnsComments("FailsMultipleTests", "The solution does not pass all tests.");
    }
}