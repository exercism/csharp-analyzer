namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal static class TestSolutionActualAnalysisResultReader
    {
        public static TestSolutionAnalysisResult Read(TestSolution solution)
        {
            var actualAnalysis = solution.ReadActualAnalysis();
            var actualMarkdown = TestSolutionMarkdownGenerator.Generate(ParseComments(actualAnalysis));

            return new TestSolutionAnalysisResult(actualAnalysis, actualMarkdown);
        }

        private static string ReadActualAnalysis(this TestSolution solution) =>
            solution.ReadFile("analysis.json");

        private static TestSolutionComment[] ParseComments(this string analysis) =>
            TestSolutionCommentsParser.ParseComments(analysis);
    }
}