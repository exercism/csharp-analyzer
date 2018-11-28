using Exercism.Analyzers.CSharp.Analysis.Analyzers.Rules;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;

namespace Exercism.Analyzers.CSharp.Tests.Analysis
{
    public class LeapAnalysisTests : ExerciseAnalysisTests
    {
        private const string Slug = "leap";

        public LeapAnalysisTests(WebApplicationFactory<Startup> factory) : base(Slug, factory)
        {
        }

        [Fact]
        public async Task AnalyzeSolutionWithMethodCanBeConvertedToExpressionBodiedMemberReturnsSingleInformationDiagnosticAsJson()
        {
            var diagnostics = await RequestAnalysis("ConvertToExpressionBodiedMember");

            Assert.Single(diagnostics, new Diagnostic("Method 'IsLeapYear' can be rewritten as an expression-bodied member.", DiagnosticLevel.Information));
        }
    }
}