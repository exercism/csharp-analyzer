using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Shared.ExpressionBodiedMember
{
    public class ExpressionBodiedMemberTests : SolutionAnalysisTests
    {
        public ExpressionBodiedMemberTests(WebApplicationFactory<Startup> factory) : base(factory, FakeExercise.Shared)
        {
        }

        [Fact]
        public Task MethodThatIsAlreadyAnExpressionBodiedMemberDoesNotReturnComment() =>
            AnalysisDoesNotReturnComment();

        [Fact]
        public Task MethodThatCanBeConvertedToExpressionBodiedMemberReturnsComment() =>
            AnalysisReturnsComment("The 'IsEven' method can be rewritten as an expression-bodied member.");
    }
}