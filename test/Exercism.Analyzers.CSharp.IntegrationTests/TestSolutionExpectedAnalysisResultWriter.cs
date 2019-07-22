namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal static class TestSolutionExpectedAnalysisResultWriter
    {
        public static void UpdateExpectedAnalysis(TestSolution solution, TestSolutionAnalysisResult analysisResult)
        {
            solution.WriteTestFile("expected_analysis.json", analysisResult.Analysis);
            solution.WriteSourceFile("expected_analysis.json", analysisResult.Analysis);
        }

        public static void UpdateExpectedComments(TestSolution solution, TestSolutionAnalysisResult analysisResult)
        {
            solution.WriteTestFile("expected_comments.md", analysisResult.Comments);
            solution.WriteSourceFile("expected_comments.md", analysisResult.Comments);
        }
    }
}