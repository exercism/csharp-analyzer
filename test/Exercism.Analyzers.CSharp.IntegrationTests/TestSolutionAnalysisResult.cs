namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal class TestSolutionAnalysisResult
    {
        public string Analysis { get; }
        public string Markdown { get; }

        public TestSolutionAnalysisResult(string analysis, string markdown) =>
            (Analysis, Markdown) = (analysis, markdown);
    }
}