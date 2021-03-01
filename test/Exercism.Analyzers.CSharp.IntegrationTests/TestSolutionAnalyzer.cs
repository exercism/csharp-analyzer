using System.Diagnostics;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal static class TestSolutionAnalyzer
    {
        public static TestSolutionAnalysisRun Run(TestSolution testSolution)
        {
            RunAnalyzer(testSolution);

            return CreateTestSolutionAnalysisRun(testSolution);
        }

        private static void RunAnalyzer(TestSolution testSolution)
        {
            if (Options.UseDocker)
                RunAnalyzerUsingDocker(testSolution);
            else
                RunAnalyzerWithoutDocker(testSolution);
        }

        private static void RunAnalyzerUsingDocker(TestSolution testSolution) =>
            Process.Start("docker", $"run -v {testSolution.DirectoryFullPath}:/solution -v {testSolution.DirectoryFullPath}:/results exercism/csharp-analyzer {testSolution.Slug} /solution /results")!.WaitForExit();

        private static void RunAnalyzerWithoutDocker(TestSolution testSolution) =>
            Program.Main(new[] { testSolution.Slug, testSolution.DirectoryFullPath, testSolution.DirectoryFullPath });

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