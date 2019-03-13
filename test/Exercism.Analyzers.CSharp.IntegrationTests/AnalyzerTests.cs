using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public abstract class AnalyzerTests
    {
        private readonly string _slug;
        private readonly string _name;

        protected AnalyzerTests(string slug, string name) =>
            (_slug, _name) = (slug, name);
        
        protected async Task ShouldBeApprovedAsOptimal(string code) =>
            await ShouldHaveStatusWithoutComment(code, "approve_as_optimal");

        protected async Task ShouldBeApprovedWithComment(string code, string comment) =>
            await ShouldHaveStatusWithComment(code, "approve_with_comment", comment);

        protected async Task ShouldBeDisapprovedWithComment(string code, string comment) =>
            await ShouldHaveStatusWithComment(code, "disapprove_with_comment", comment);

        protected async Task ShouldBeReferredToMentor(string code) =>
            await ShouldHaveStatusWithoutComment(code, "refer_to_mentor");

        private async Task ShouldHaveStatusWithoutComment(string code, string status)
        {
            var analysisRun = await Analyze(code);

            Assert.Equal(status, analysisRun.Status);
            Assert.Empty(analysisRun.Comments);
        }

        private async Task ShouldHaveStatusWithComment(string code, string status, string comment)
        {
            var analysisRun = await Analyze(code);

            Assert.Equal(status, analysisRun.Status);
            Assert.Single(analysisRun.Comments, comment);
        }

        private async Task<TestSolutionAnalysisRun> Analyze(string code) =>
            await TestSolutionAnalyzer.Run(_slug, _name, code);
    }
}