using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Gigasecond.ExponentNotation
{
    public class ExponentNotationTests : SolutionAnalysisTests
    {
        public ExponentNotationTests(WebApplicationFactory<Startup> factory) : base(factory, FakeExercise.Gigasecond)
        {
        }

        [Fact]
        public async Task UsingExponentNotationDoesNotReturnComment() =>
            await AnalysisDoesNotReturnComment();

        [Fact]
        public async Task UsingIntegerNotationReturnsComment() =>
            await AnalysisReturnsComment("You can write `1000000000` as `1e9`.");

        [Fact]
        public async Task UsingIntegerNotationWithSeparatorReturnsComment() =>
            await AnalysisReturnsComment("You can write `1_000_000_000` as `1e9`.");
    }
}