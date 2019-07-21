namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal static class TestSolutionExpectedAnalysisResultWriter
    {
        public static void UpdateExpectedAnalysis(TestSolution solution, TestSolutionAnalysisResult analysisResult) =>
            solution.WriteFile("expected_analysis.json", analysisResult.Analysis);

        public static void UpdateExpectedComments(TestSolution solution, TestSolutionAnalysisResult analysisResult) =>
            solution.WriteFile("expected_comments.md", analysisResult.Comments);
    }
}