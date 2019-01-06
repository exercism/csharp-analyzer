using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Shared.DigitSeparator
{
    public class DigitSeparatorTests : SolutionAnalysisTests
    {
        public DigitSeparatorTests(WebApplicationFactory<Startup> factory) : base(factory, FakeExercise.Shared)
        {
        }

        [Fact]
        public async Task VeryLargeDecimalNumberWithoutDigitSeparatorReturnsComment() =>
            await AnalysisReturnsComment(expected: "You can write `1000000000` as `1_000_000_000`.");

        [Fact]
        public async Task VeryLargeBinaryNumberWithoutDigitSeparatorReturnsComment() =>
            await AnalysisReturnsComment(expected: "You can write `0b100100100011` as `0b1001_0010_0011`.");

        [Fact]
        public async Task VeryLargeHexadecimalNumberWithoutDigitSeparatorReturnsComment() =>
            await AnalysisReturnsComment(expected: "You can write `0x1AFE8DFF` as `0x1A_FE_8D_FF`.");

        [Fact]
        public async Task LargeDecimalNumberWithoutDigitSeparatorReturnsComment() =>
            await AnalysisReturnsComment(expected: "You can write `100000` as `100_000`.");

        [Fact]
        public async Task LargeBinaryNumberWithoutDigitSeparatorReturnsComment() =>
            await AnalysisReturnsComment(expected: "You can write `0b10010010` as `0b1001_0010`.");

        [Fact]
        public async Task LargeHexadecimalNumberWithoutDigitSeparatorReturnsComment() =>
            await AnalysisReturnsComment(expected: "You can write `0x1AFE` as `0x1A_FE`.");

        [Fact]
        public async Task LargeDecimalNumberWithDigitSeparatorDoesNotReturnComment() =>
            await AnalysisDoesNotReturnComment();

        [Fact]
        public async Task LargeHexadecimalNumberWithDigitSeparatorDoesNotReturnComment() =>
            await AnalysisDoesNotReturnComment();

        [Fact]
        public async Task LargeBinaryNumberWithDigitSeparatorDoesNotReturnComment() =>
            await AnalysisDoesNotReturnComment();

        [Fact]
        public async Task SmallDecimalNumberWithoutDigitSeparatorDoesNotReturnComment() =>
            await AnalysisDoesNotReturnComment();

        [Fact]
        public async Task SmallHexadecimalNumberWithoutDigitSeparatorDoesNotReturnComment() =>
            await AnalysisDoesNotReturnComment();

        [Fact]
        public async Task SmallBinaryNumberWithoutDigitSeparatorDoesNotReturnComment() =>
            await AnalysisDoesNotReturnComment();
    }
}