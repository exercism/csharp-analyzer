namespace Exercism.Analyzers.CSharp.IntegrationTests.Helpers
{
    public class TestSolutionAnalysisRun
    {
        public bool Success { get; }
        public bool Approved { get; }
        public bool ReferToMentor { get; }
        public string[] Messages { get; }

        public TestSolutionAnalysisRun(int returnCode, bool approved, bool referToMentor, string[] messages)
        {
            Success = returnCode == 0;
            Approved = approved;
            ReferToMentor = referToMentor;
            Messages = messages;
        }
    }
}