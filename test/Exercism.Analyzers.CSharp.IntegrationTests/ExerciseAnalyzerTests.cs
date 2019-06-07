using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class ExerciseAnalyzerTests
    {
        [Theory]
        [TestSolutionsData]
        public void SolutionShouldBeCorrectlyAnalyzed(TestSolution testSolution)
        {
            var analysisRun = TestSolutionAnalyzer.Run(testSolution);

            Assert.True(analysisRun.Expected.Status == analysisRun.Actual.Status, testSolution.Directory);
            foreach(var expectedComment in analysisRun.Expected.Comments)
            {
                Assert.Contains(expectedComment, analysisRun.Actual.Comments);
            }
        }
    }
}