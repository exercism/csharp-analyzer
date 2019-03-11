namespace Exercism.Analyzers.CSharp.IntegrationTests.Helpers
{
    public class TestSolutionAnalysisRun
    {
        public bool Success { get; }
        public string Status { get; }
        public string[] Comments { get; }

        public bool ApproveAsOptimal => Status == TestSolutionAnalysisStatus.ApproveAsOptimal;
        public bool ApproveWithComment => Status == TestSolutionAnalysisStatus.ApproveWithComment;
        public bool DisapproveWithComment => Status == TestSolutionAnalysisStatus.DisapproveWithComment;
        public bool ReferToMentor => Status == TestSolutionAnalysisStatus.ReferToMentor;

        public TestSolutionAnalysisRun(int returnCode, string status, string[] comments)
        {
            Success = returnCode == 0;
            Status = status;
            Comments = comments;
        }
    }
}