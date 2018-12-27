using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Leap
{
    public class LeapNumberOfChecksTests : SolutionAnalysisTests
    {
        public LeapNumberOfChecksTests(WebApplicationFactory<Startup> factory) : base(factory, FakeExercise.Leap)
        {
        }

        [Fact]
        public Task UsingMinimumNumberOfChecksDoesNotReturnComments() =>
            AnalysisReturnsNoComments("UsingMinimumNumberOfChecks");

        [Fact]
        public Task UsingTooManyChecksReturnsComment() =>
            AnalysisReturnsComments("UsingTooManyChecks", "The 'IsLeapYear' method uses too many checks.");
    }
}