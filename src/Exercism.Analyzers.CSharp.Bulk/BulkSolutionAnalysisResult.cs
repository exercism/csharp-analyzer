namespace Exercism.Analyzers.CSharp.Bulk
{
    internal class BulkSolutionAnalysisResult
    {
        public string Status { get; }
        public string[] Comments { get; }

        public BulkSolutionAnalysisResult(string status, string[] comments) =>
            (Status, Comments) = (status, comments);
    }
}