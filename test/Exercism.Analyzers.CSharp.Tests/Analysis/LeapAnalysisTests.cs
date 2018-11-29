using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Analyzers;
using Xunit;

namespace Exercism.Analyzers.CSharp.Tests.Analysis
{
    public class LeapAnalysisTests : ExerciseAnalysisTests
    {
        private const string Slug = "leap";

        public LeapAnalysisTests(WebApplicationFactory<Startup> factory) : base(Slug, factory)
        {
        }

        [Fact]
        public Task AnalyzeSolutionWithIsLeapYearUsesCorrectNumberOfChecksDoesNotReturnComments()
            => VerifyReturnsNoComments("CorrectNumberOfChecks");

        [Fact]
        public Task AnalyzeSolutionWithIsLeapYearUsesTooManyChecksReturnsComment() 
            => VerifyReturnsComments("TooManyChecks", "The 'IsLeapYear' method uses too many checks.");
    }
}