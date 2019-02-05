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
        public async Task MethodThatIsAlreadyAnExpressionBodiedMemberRequiresMentoringWithoutComments() =>
            await RequiresMentoringWithoutComments();

        [Fact]
        public async Task SingleLineMethodThatCannotBeConvertedToExpressionBodiedMemberRequiresMentoringWithoutComments() =>
            await RequiresMentoringWithoutComments();

        [Fact]
        public async Task MultiLineMethodThatCannotBeConvertedToExpressionBodiedMemberRequiresMentoringWithoutComments() =>
            await RequiresMentoringWithoutComments();

        [Fact]
        public async Task SingleLineMethodThatCanBeConvertedToExpressionBodiedMemberRequiresMentoringWithComment() =>
            await RequiresMentoringWithComment("The 'IsEven' method can be rewritten as an expression-bodied member.");

        [Fact]
        public async Task MultiLineMethodThatCanBeConvertedToExpressionBodiedMemberRequiresMentoringWithComment() =>
            await RequiresMentoringWithComment("The 'IsEven' method can be rewritten as an expression-bodied member.");
    }
}