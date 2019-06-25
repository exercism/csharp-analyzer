namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal class TestSolutionAnalysisResult
    {
        public string Status { get; }
        public TestSolutionComment[] Comments { get; }

        public TestSolutionAnalysisResult(string status, TestSolutionComment[] comments) =>
            (Status, Comments) = (status, comments);
    }
}