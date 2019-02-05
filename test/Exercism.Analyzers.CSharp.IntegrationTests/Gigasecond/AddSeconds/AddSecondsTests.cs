using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Gigasecond.AddSeconds
{
    public class AddSecondsTests : SolutionAnalysisTests
    {
        public AddSecondsTests(WebApplicationFactory<Startup> factory) : base(factory, FakeExercise.Gigasecond)
        {
        }

        [Fact]
        public async Task UsingAddSecondsApprovedWithoutComments() =>
            await ApprovedWithoutComments();

        [Fact]
        public async Task UsingAddRequiresChangeWithComment() =>
            await RequiresChangeWithComment("You could use `AddSeconds()`.");

        [Fact]
        public async Task UsingPlusOperatorRequiresChangeWithComment() =>
            await RequiresChangeWithComment("You could use `AddSeconds()`.");
    }
}