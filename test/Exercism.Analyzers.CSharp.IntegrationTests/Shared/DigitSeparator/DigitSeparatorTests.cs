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
        public async Task VeryLargeDecimalNumberWithoutDigitSeparatorRequiresMentoringWithComment() =>
            await RequiresMentoringWithComment(expected: "You can write `1000000000` as `1_000_000_000`.");

        [Fact]
        public async Task VeryLargeBinaryNumberWithoutDigitSeparatorRequiresMentoringWithComment() =>
            await RequiresMentoringWithComment(expected: "You can write `0b100100100011` as `0b1001_0010_0011`.");

        [Fact]
        public async Task VeryLargeHexadecimalNumberWithoutDigitSeparatorRequiresMentoringWithComment() =>
            await RequiresMentoringWithComment(expected: "You can write `0x1AFE8DFF` as `0x1A_FE_8D_FF`.");

        [Fact]
        public async Task LargeDecimalNumberWithoutDigitSeparatorRequiresMentoringWithComment() =>
            await RequiresMentoringWithComment(expected: "You can write `100000` as `100_000`.");

        [Fact]
        public async Task LargeBinaryNumberWithoutDigitSeparatorRequiresMentoringWithComment() =>
            await RequiresMentoringWithComment(expected: "You can write `0b10010010` as `0b1001_0010`.");

        [Fact]
        public async Task LargeHexadecimalNumberWithoutDigitSeparatorRequiresMentoringWithComment() =>
            await RequiresMentoringWithComment(expected: "You can write `0x1AFE` as `0x1A_FE`.");

        [Fact]
        public async Task LargeDecimalNumberWithDigitSeparatorRequiresMentoringWithoutComments() =>
            await RequiresMentoringWithoutComments();

        [Fact]
        public async Task LargeHexadecimalNumberWithDigitSeparatorRequiresMentoringWithoutComments() =>
            await RequiresMentoringWithoutComments();

        [Fact]
        public async Task LargeBinaryNumberWithDigitSeparatorRequiresMentoringWithoutComments() =>
            await RequiresMentoringWithoutComments();

        [Fact]
        public async Task SmallDecimalNumberWithoutDigitSeparatorRequiresMentoringWithoutComments() =>
            await RequiresMentoringWithoutComments();

        [Fact]
        public async Task SmallHexadecimalNumberWithoutDigitSeparatorRequiresMentoringWithoutComments() =>
            await RequiresMentoringWithoutComments();

        [Fact]
        public async Task SmallBinaryNumberWithoutDigitSeparatorRequiresMentoringWithoutComments() =>
            await RequiresMentoringWithoutComments();
    }
}