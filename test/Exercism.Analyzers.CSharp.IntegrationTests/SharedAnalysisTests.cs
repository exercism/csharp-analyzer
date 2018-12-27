using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class SharedAnalysisTests : ExerciseAnalysisTests
    {
        public SharedAnalysisTests(WebApplicationFactory<Startup> factory) : base(Exercise.Shared, factory)
        {
        }
        
        [Fact]
        public Task NoDiagnosticsDoesNotReturnComments() =>
            AnalysisReturnsNoComments("NoDiagnostics");

        [Fact]
        public Task CompileErrorsReturnsComment() =>
            AnalysisReturnsComments("DoesNotCompile", "The solution does not compile.");

        [Fact]
        public Task FailingTestsReturnsComment() =>
            AnalysisReturnsComments("FailingTests", "The solution does not pass all tests.");

        [Fact]
        public Task MethodThatCanBeConvertedToExpressionBodiedMemberReturnsComment() =>
            AnalysisReturnsComments("ConvertToExpressionBodiedMember", "The 'IsEven' method can be rewritten as an expression-bodied member.");
    }
}