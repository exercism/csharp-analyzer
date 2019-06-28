using DeepEqual.Syntax;
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
            analysisRun.Actual.ShouldDeepEqual(analysisRun.Expected);
        }
    }
}