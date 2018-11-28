using Microsoft.AspNetCore.Mvc.Testing;

namespace Exercism.Analyzers.CSharp.Tests.Analysis
{
    public class LeapAnalysisTests : ExerciseAnalysisTests
    {
        private const string Slug = "leap";

        public LeapAnalysisTests(WebApplicationFactory<Startup> factory) : base(Slug, factory)
        {
        }
    }
}