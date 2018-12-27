using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Shared
{
    public class SharedCompilationTests : SolutionAnalysisTests
    {
        public SharedCompilationTests(WebApplicationFactory<Startup> factory) : base(factory, FakeExercise.Shared)
        {
        }

        [Fact]
        public Task NoDiagnosticsDoesNotReturnComments() =>
            AnalysisReturnsNoComments("NoDiagnostics");

        [Fact]
        public Task CompileErrorsReturnsComment() =>
            AnalysisReturnsComments("DoesNotCompile", "The solution does not compile.");
    }
}