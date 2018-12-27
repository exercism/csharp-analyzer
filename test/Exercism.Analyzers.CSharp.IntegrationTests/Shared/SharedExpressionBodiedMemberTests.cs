using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Shared
{
    public class SharedExpressionBodiedMemberTests : SolutionAnalysisTests
    {
        public SharedExpressionBodiedMemberTests(WebApplicationFactory<Startup> factory) : base(factory, FakeExercise.Shared)
        {
        }

        [Fact]
        public Task MethodThatCanBeConvertedToExpressionBodiedMemberReturnsComment() =>
            AnalysisReturnsComments("ConvertToExpressionBodiedMember", "The 'IsEven' method can be rewritten as an expression-bodied member.");
    }
}