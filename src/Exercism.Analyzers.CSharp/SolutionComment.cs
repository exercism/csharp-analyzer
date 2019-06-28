namespace Exercism.Analyzers.CSharp
{
    internal class SolutionComment
    {
        public string Comment { get; }
        public SolutionCommentParameter[] Parameters { get; }

        public SolutionComment(string comment, SolutionCommentParameter[] parameters) =>
            (Comment, Parameters) = (comment, parameters);
    }
}