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
        public Task UsingExponentNotationDoesNotReturnComment() =>
            AnalysisReturnsNoComments("UsingExponentNotation");

        [Fact]
        public Task UsingIntegerNotationReturnsComment() =>
            AnalysisReturnsComments("UsingIntegerNotation", "You can write `1000000000` as `1e9`.");

        [Fact]
        public Task UsingIntegerNotationWithSeparatorReturnsComment() =>
            AnalysisReturnsComments("UsingIntegerNotationWithSeparator", "You can write `1_000_000_000` as `1e9`.");
    }
}