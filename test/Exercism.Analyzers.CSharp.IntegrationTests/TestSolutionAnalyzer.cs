using System.IO;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal static class TestSolutionAnalyzer
    {
        public static TestSolutionAnalysisRun Run(TestSolution testSolution)
        {
            Program.Main(new[] { testSolution.Slug, testSolution.Directory });

            return CreateTestSolutionAnalyisRun(testSolution);
        }

        private static TestSolutionAnalysisRun CreateTestSolutionAnalyisRun(TestSolution solution)
        {
            var expectedAnalysisJsonFilePath = Path.Combine(solution.Directory, "expected_analysis.json");
            var actualAnalysisJsonFilePath = Path.Combine(solution.Directory, "analysis.json");

            var expectedAnalysisResult = TestSolutionAnalysisResultReader.Read(expectedAnalysisJsonFilePath);
            var actualAnalysisResult = TestSolutionAnalysisResultReader.Read(actualAnalysisJsonFilePath);

            return new TestSolutionAnalysisRun(expectedAnalysisResult, actualAnalysisResult);
        }
    }
}