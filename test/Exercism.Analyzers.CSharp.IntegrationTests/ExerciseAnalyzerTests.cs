using System.Threading.Tasks;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class ExerciseAnalyzerTests
    {
        [Theory]
        [TestSolutionsDataAttribute]
        public async Task SolutionShouldBeCorrectlyAnalyzed(TestSolution testSolution)
        {
            var analysisRun = await TestSolutionAnalyzer.Run(testSolution);

            Assert.Equal(analysisRun.Expected.Status, analysisRun.Actual.Status);
            Assert.Equal(analysisRun.Expected.Comments, analysisRun.Actual.Comments);
        }
    }
}