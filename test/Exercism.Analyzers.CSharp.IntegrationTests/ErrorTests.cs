using System.Threading.Tasks;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class ErrorTests : AnalyzerTests
    {
        public ErrorTests() : base("errors", "Errors")
        {
        }

        [Fact]
        public async Task DisapproveWithCommentWhenCodeHasSyntaxErrors()
        {
            const string code = @"
                public static class Gigasecond
                {
                    public static DateTime Add
                }";

            await ShouldBeDisapprovedWithComment(code, "csharp.general.has_compile_errors");
        }
    }
}