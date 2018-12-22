using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Analyzers;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Xunit;

namespace Exercism.Analyzers.CSharp.Tests.Analysis
{
    public class LeapAnalysisTests : ExerciseAnalysisTests
    {
        public LeapAnalysisTests(WebApplicationFactory<Startup> factory) : base(Exercise.Leap, factory)
        {
        }

        [Fact]
        public Task AnalyzeSolutionWithMinimumNumberOfChecksDoesNotReturnComments()
            => VerifyReturnsNoComments("WithMinimumNumberOfChecks");

        [Fact]
        public Task AnalyzeSolutionWithTooManyChecksReturnsComment() 
            => VerifyReturnsComments("WithTooManyChecks", "The 'IsLeapYear' method uses too many checks.");
    }
}