using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class ExerciseAnalyzerTests
    {
        [Theory]
        [TestSolutionsData]
        public void SolutionIsAnalyzedCorrectly(TestSolution testSolution)
        {
            var analysisRun = TestSolutionAnalyzer.Run(testSolution);
            Assert.Equal(analysisRun.Expected.Analysis.NormalizeJson(), analysisRun.Actual.Analysis.NormalizeJson());
            Assert.Equal(analysisRun.Expected.Markdown.NormalizeMarkdown(), analysisRun.Actual.Markdown.NormalizeMarkdown());
        }
    }
}