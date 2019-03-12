namespace Exercism.Analyzers.CSharp.IntegrationTests.Helpers
{
    public class TestSolutionAnalysisRun
    {
        public string Status { get; }
        public string[] Comments { get; }

        public bool ApproveAsOptimal => Status == "approve_as_optimal";
        public bool ApproveWithComment => Status == "approve_with_comment";
        public bool DisapproveWithComment => Status == "disapprove_with_comment";
        public bool ReferToMentor => Status == "refer_to_mentor";

        public TestSolutionAnalysisRun(string status, string[] comments) =>
            (Status, Comments) = (status, comments);
    }
}