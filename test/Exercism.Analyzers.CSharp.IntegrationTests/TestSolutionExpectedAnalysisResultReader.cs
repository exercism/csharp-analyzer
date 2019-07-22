namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal static class TestSolutionExpectedAnalysisResultReader
    {
        public static TestSolutionAnalysisResult Read(TestSolution solution) =>
            new TestSolutionAnalysisResult(solution.ReadExpectedAnalysis(), solution.ReadExpectedComments());

        private static string ReadExpectedAnalysis(this TestSolution solution) =>
            solution.ReadTestFile("expected_analysis.json");

        private static string ReadExpectedComments(this TestSolution solution) =>
            solution.ReadTestFile("expected_comments.md");
    }
}