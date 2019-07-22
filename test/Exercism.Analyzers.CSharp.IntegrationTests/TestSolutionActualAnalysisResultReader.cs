namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal static class TestSolutionActualAnalysisResultReader
    {
        public static TestSolutionAnalysisResult Read(TestSolution solution)
        {
            var actualAnalysis = solution.ReadActualAnalysis();
            var actualComments = TestSolutionCommentsMarkdownGenerator.Generate(ParseComments(actualAnalysis));

            return new TestSolutionAnalysisResult(actualAnalysis, actualComments);
        }

        private static string ReadActualAnalysis(this TestSolution solution) =>
            solution.ReadTestFile("analysis.json");

        private static TestSolutionComment[] ParseComments(this string analysis) =>
            TestSolutionCommentsParser.ParseComments(analysis);
    }
}