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
        public Task VeryLargeDecimalNumberWithoutDigitSeparatorReturnsComment() =>
            AnalysisReturnsComment(expected: "You can write `1000000000` as `1_000_000_000`.");

        [Fact]
        public Task VeryLargeBinaryNumberWithoutDigitSeparatorReturnsComment() =>
            AnalysisReturnsComment(expected: "You can write `0b100100100011` as `0b1001_0010_0011`.");

        [Fact]
        public Task VeryLargeHexadecimalNumberWithoutDigitSeparatorReturnsComment() =>
            AnalysisReturnsComment(expected: "You can write `0x1AFE8DFF` as `0x1A_FE_8D_FF`.");

        [Fact]
        public Task LargeDecimalNumberWithoutDigitSeparatorReturnsComment() =>
            AnalysisReturnsComment(expected: "You can write `100000` as `100_000`.");

        [Fact]
        public Task LargeBinaryNumberWithoutDigitSeparatorReturnsComment() =>
            AnalysisReturnsComment(expected: "You can write `0b10010010` as `0b1001_0010`.");

        [Fact]
        public Task LargeHexadecimalNumberWithoutDigitSeparatorReturnsComment() =>
            AnalysisReturnsComment(expected: "You can write `0x1AFE` as `0x1A_FE`.");

        [Fact]
        public Task LargeDecimalNumberWithDigitSeparatorDoesNotReturnComment() =>
            AnalysisDoesNotReturnComment();

        [Fact]
        public Task LargeHexadecimalNumberWithDigitSeparatorDoesNotReturnComment() =>
            AnalysisDoesNotReturnComment();

        [Fact]
        public Task LargeBinaryNumberWithDigitSeparatorDoesNotReturnComment() =>
            AnalysisDoesNotReturnComment();

        [Fact]
        public Task SmallDecimalNumberWithoutDigitSeparatorDoesNotReturnComment() =>
            AnalysisDoesNotReturnComment();

        [Fact]
        public Task SmallHexadecimalNumberWithoutDigitSeparatorDoesNotReturnComment() =>
            AnalysisDoesNotReturnComment();

        [Fact]
        public Task SmallBinaryNumberWithoutDigitSeparatorDoesNotReturnComment() =>
            AnalysisDoesNotReturnComment();
    }
}