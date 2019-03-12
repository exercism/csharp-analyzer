namespace Exercism.Analyzers.CSharp.IntegrationTests.Helpers
{
    public class TestSolutionAnalysisRun
    {
        public string Status { get; }
        public string[] Comments { get; }

        public TestSolutionAnalysisRun(string status, string[] comments) =>
            (Status, Comments) = (status, comments);
    }
}