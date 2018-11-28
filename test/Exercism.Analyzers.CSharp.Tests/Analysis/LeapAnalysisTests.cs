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
        public async Task AnalyzeSolutionWithIsLeapYearUsesTooManyChecksReturnsSingleWarningDiagnosticAsJson()
        {
            var diagnostics = await RequestAnalysis("TooManyChecks");

            Assert.Single(diagnostics, new Diagnostic("The `IsLeapYear` method uses too many checks.", DiagnosticLevel.Warning));
        }
    }
}