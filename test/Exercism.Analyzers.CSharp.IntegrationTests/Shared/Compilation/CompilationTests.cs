using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Shared.Compilation
{
    public class CompilationTests : SolutionAnalysisTests
    {
        public CompilationTests(WebApplicationFactory<Startup> factory) : base(factory, FakeExercise.Shared)
        {
        }

        [Fact]
        public Task CompilesWithoutErrorsDoesNotReturnComment() =>
            AnalysisDoesNotReturnComment();

        [Fact]
        public Task CompilesWithErrorsReturnsComment() =>
            AnalysisReturnsComment("The solution does not compile.");

        [Fact]
        public Task CompilesWithErrorsAndNonErrorsReturnsSingleComment() =>
            AnalysisReturnsSingleComment("The solution does not compile.");
    }
}