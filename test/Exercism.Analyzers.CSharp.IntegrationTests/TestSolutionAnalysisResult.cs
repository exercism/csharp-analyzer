namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal class TestSolutionAnalysisResult
    {
        public string Analysis { get; }

        public TestSolutionAnalysisResult(string analysis) => Analysis = analysis;
    }
}