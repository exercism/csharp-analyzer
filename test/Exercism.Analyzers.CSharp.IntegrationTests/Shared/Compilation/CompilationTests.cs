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
            AnalysisReturnsNoComments("CompilesWithoutErrors");

        [Fact]
        public Task CompilesWithErrorsReturnsComment() =>
            AnalysisReturnsComments("CompilesWithErrors", "The solution does not compile.");
    }
}