using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class GigasecondAnalysisTests : ExerciseAnalysisTests
    {
        public GigasecondAnalysisTests(WebApplicationFactory<Startup> factory) : base(Exercise.Gigasecond, factory)
        {
        }

        [Fact]
        public Task SolutionUsingExponentNotationDoesNotReturnComments() =>
            AnalysisReturnsNoComments("UsingExponentNotation");

        [Fact]
        public Task SolutionUsingIntegerNotationReturnsComment() =>
            AnalysisReturnsComments("UsingIntegerNotation", "You can write `1000000000` as `1e9`.");

        [Fact]
        public Task UsingIntegerNotationWithSeparatorReturnsComment() =>
            AnalysisReturnsComments("UsingIntegerNotationWithSeparator", "You can write `1_000_000_000` as `1e9`.");
    }
}