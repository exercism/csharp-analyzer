namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal static class TestSolutionAnalyzer
    {
        public static TestSolutionAnalysisRun Run(TestSolution testSolution)
        {
            Program.Main(new[] { testSolution.Slug, testSolution.Directory, testSolution.Directory });

            return CreateTestSolutionAnalysisRun(testSolution);
        }

        private static TestSolutionAnalysisRun CreateTestSolutionAnalysisRun(TestSolution solution)
        {
            var actualAnalysisResult = TestSolutionActualAnalysisResultReader.Read(solution);

            UpdateExpectedAnalysisResultIfNeeded(solution, actualAnalysisResult);

            var expectedAnalysisResult = TestSolutionExpectedAnalysisResultReader.Read(solution);

            return new TestSolutionAnalysisRun(expectedAnalysisResult, actualAnalysisResult);
        }

        private static void UpdateExpectedAnalysisResultIfNeeded(TestSolution solution, TestSolutionAnalysisResult actualAnalysisResult)
        {
            if (Options.UpdateAnalysis)
                TestSolutionExpectedAnalysisResultWriter.UpdateExpectedAnalysis(solution, actualAnalysisResult);

            if (Options.UpdateComments)
                TestSolutionExpectedAnalysisResultWriter.UpdateExpectedComments(solution, actualAnalysisResult);
        }
    }
}