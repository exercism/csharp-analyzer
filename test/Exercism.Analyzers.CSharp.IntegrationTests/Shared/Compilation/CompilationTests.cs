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
        public async Task CompilesWithoutErrorsRequiresMentoringWithoutComments() =>
            await RequiresMentoringWithoutComments();

        [Fact]
        public async Task CompilesWithErrorsRequiresChangeWithComment() =>
            await RequiresChangeWithSingleComment("The solution does not compile.");

        [Fact]
        public async Task CompilesWithErrorsAndNonErrorsRequiresChangeWithSingleComment() =>
            await RequiresChangeWithSingleComment("The solution does not compile.");
    }
}