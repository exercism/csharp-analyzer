using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Shared.ExponentNotation
{
    public class ExponentNotationTests : SolutionAnalysisTests
    {
        public ExponentNotationTests(WebApplicationFactory<Startup> factory) : base(factory, FakeExercise.Shared)
        {
        }

        [Fact]
        public async Task LargeDoubleLiteralNotUsingExponentNotationReturnsComment() =>
            await AnalysisReturnsComment(expected: "You can write `1000000d` as `1e6`.");

        [Fact]
        public async Task LargeDoubleArgumentNotUsingExponentNotationReturnsComment() =>
            await AnalysisReturnsComment(expected: "You can write `1000000d` as `1e6`.");

        [Fact]
        public async Task LargeIntegerArgumentImplicitlyConvertedToDoubleReturnsComment() =>
            await AnalysisReturnsComment(expected: "You can write `1000000` as `1e6`.");

        [Fact]
        public async Task LargeDoubleUsingExponentNotationDoesNotReturnComment() =>
            await AnalysisDoesNotReturnComment();

        [Fact]
        public async Task SmallDoubleNotUsingExponentNotationDoesNotReturnComment() =>
            await AnalysisDoesNotReturnComment();

        [Fact]
        public async Task LargeFloatDoesNotReturnComment() =>
            await AnalysisDoesNotReturnComment();

        [Fact]
        public async Task SmallFloatDoesNotReturnComment() =>
            await AnalysisDoesNotReturnComment();
    }
}