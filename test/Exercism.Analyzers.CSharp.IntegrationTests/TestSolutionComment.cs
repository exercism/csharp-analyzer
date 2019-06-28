namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal class TestSolutionComment
    {
        public string Comment { get; }
        public TestSolutionCommentParameter[] Parameters { get; }

        public TestSolutionComment(string comment, TestSolutionCommentParameter[] parameters) =>
            (Comment, Parameters) = (comment, parameters);
    }
}