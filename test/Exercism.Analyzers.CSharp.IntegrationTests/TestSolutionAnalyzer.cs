namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal static class TestSolutionAnalyzer
    {
        public static TestSolutionAnalysisRun Run(TestSolution testSolution)
        {
            Program.Main(new[] { testSolution.Slug, testSolution.Directory });

            return CreateTestSolutionAnalysisRun(testSolution);
        }

        private static TestSolutionAnalysisRun CreateTestSolutionAnalysisRun(TestSolution solution)
        {
            var expectedAnalysisResult = TestSolutionExpectedAnalysisResultReader.Read(solution);
            var actualAnalysisResult = TestSolutionActualAnalysisResultReader.Read(solution);

            return new TestSolutionAnalysisRun(expectedAnalysisResult, actualAnalysisResult);
        }
    }
}