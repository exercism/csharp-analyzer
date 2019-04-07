namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal class TestSolutionAnalysisRun
    {
        public TestSolutionAnalysisResult Expected { get; }
        public TestSolutionAnalysisResult Actual { get; }

        public TestSolutionAnalysisRun(TestSolutionAnalysisResult expected, TestSolutionAnalysisResult actual) =>
            (Expected, Actual) = (expected, actual);
    }
}