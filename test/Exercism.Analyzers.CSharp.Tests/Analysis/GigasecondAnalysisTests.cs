using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Xunit;

namespace Exercism.Analyzers.CSharp.Tests.Analysis
{
    public class GigasecondAnalysisTests : ExerciseAnalysisTests
    {
        public GigasecondAnalysisTests(WebApplicationFactory<Startup> factory) : base(Exercise.Gigasecond, factory)
        {
        }

        [Fact]
        public Task AnalyzeSolutionUsingExponentNotationDoesNotReturnComments()
            => VerifyReturnsNoComments("UsingExponentNotation");

        [Fact]
        public Task AnalyzeSolutionUsingIntegerNotationReturnsComment() 
            => VerifyReturnsComments("UsingIntegerNotation", "You can write `1000000000` as `1e9`.");

        [Fact]
        public Task AnalyzeSolutionUsingIntegerNotationWithSeparatorReturnsComment() 
            => VerifyReturnsComments("UsingIntegerNotationWithSeparator", "You can write `1_000_000_000` as `1e9`.");
    }
}