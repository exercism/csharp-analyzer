namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal class TestSolutionAnalysisResult
    {
        public string Status { get; }
        public string[] Comments { get; }

        public TestSolutionAnalysisResult(string status, string[] comments) =>
            (Status, Comments) = (status, comments);
    }
}