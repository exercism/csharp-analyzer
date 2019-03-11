namespace Exercism.Analyzers.CSharp.IntegrationTests.Helpers
{
    public class TestSolutionAnalysisRun
    {
        public string Status { get; }
        public string[] Comments { get; }

        public bool ApproveAsOptimal => Status == TestSolutionAnalysisStatus.ApproveAsOptimal;
        public bool ApproveWithComment => Status == TestSolutionAnalysisStatus.ApproveWithComment;
        public bool DisapproveWithComment => Status == TestSolutionAnalysisStatus.DisapproveWithComment;
        public bool ReferToMentor => Status == TestSolutionAnalysisStatus.ReferToMentor;

        public TestSolutionAnalysisRun(string status, string[] comments) =>
            (Status, Comments) = (status, comments);
    }
}