namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal static class TestSolutionActualAnalysisResultReader
    {
        public static TestSolutionAnalysisResult Read(TestSolution solution)
        {
            var actualAnalysis = solution.ReadActualAnalysis();

            return new TestSolutionAnalysisResult(actualAnalysis);
        }

        private static string ReadActualAnalysis(this TestSolution solution) =>
            solution.ReadTestFile("analysis.json");
    }
}