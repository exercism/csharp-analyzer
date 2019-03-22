using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class ExerciseAnalyzerTests
    {
        [Theory]
        [TestSolutionsDataAttribute]
        public void SolutionShouldBeCorrectlyAnalyzed(TestSolution testSolution)
        {
            var analysisRun = TestSolutionAnalyzer.Run(testSolution);

            Assert.Equal(analysisRun.Expected.Status, analysisRun.Actual.Status);
            Assert.Equal(analysisRun.Expected.Comments, analysisRun.Actual.Comments);
        }
    }
}