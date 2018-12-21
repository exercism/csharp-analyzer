using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Xunit;

namespace Exercism.Analyzers.CSharp.Tests.Analysis
{
    public class GigasecondAnalysisTests : ExerciseAnalysisTests
    {
        public GigasecondAnalysisTests(WebApplicationFactory<Startup> factory) : base(Exercise.Gigasecond, factory)
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