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
        public async Task LargeDoubleLiteralNotUsingExponentNotationRequiresMentoringWithComment() =>
            await RequiresMentoringWithComment(expected: "You can write `1000000d` as `1e6`.");

        [Fact]
        public async Task LargeDoubleArgumentNotUsingExponentNotationRequiresMentoringWithComment() =>
            await RequiresMentoringWithComment(expected: "You can write `1000000d` as `1e6`.");

        [Fact]
        public async Task LargeIntegerArgumentImplicitlyConvertedToDoubleRequiresMentoringWithComment() =>
            await RequiresMentoringWithComment(expected: "You can write `1000000` as `1e6`.");

        [Fact]
        public async Task LargeDoubleUsingExponentNotationRequiresMentoringWithoutComments() =>
            await RequiresMentoringWithoutComments();

        [Fact]
        public async Task SmallDoubleNotUsingExponentNotationRequiresMentoringWithoutComments() =>
            await RequiresMentoringWithoutComments();

        [Fact]
        public async Task LargeFloatRequiresMentoringWithoutComments() =>
            await RequiresMentoringWithoutComments();

        [Fact]
        public async Task SmallFloatRequiresMentoringWithoutComments() =>
            await RequiresMentoringWithoutComments();
    }
}