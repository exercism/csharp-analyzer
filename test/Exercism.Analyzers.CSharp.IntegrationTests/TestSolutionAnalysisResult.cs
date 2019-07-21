namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal class TestSolutionAnalysisResult
    {
        public string Analysis { get; }
        public string Comments { get; }

        public TestSolutionAnalysisResult(string analysis, string comments) =>
            (Analysis, Comments) = (analysis, comments);
    }
}