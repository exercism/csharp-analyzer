namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal static class TestSolutionExpectedAnalysisResultReader
    {
        public static TestSolutionAnalysisResult Read(TestSolution solution) => 
            new TestSolutionAnalysisResult(solution.ReadExpectedAnalysis(), solution.ReadExpectedMarkdown());

        private static string ReadExpectedAnalysis(this TestSolution solution) =>
            solution.ReadFile("expected_analysis.json");

        private static string ReadExpectedMarkdown(this TestSolution solution) =>
            solution.ReadFile("expected_comments.md");
    }
}