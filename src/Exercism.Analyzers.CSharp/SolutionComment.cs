namespace Exercism.Analyzers.CSharp;

internal enum SolutionCommentType
{
    Essential,
    Actionable,
    Informative,
    Celebratory
}

internal record SolutionCommentParameter(string Key, string Value);

internal record SolutionComment(string Comment, SolutionCommentType Type, params SolutionCommentParameter[] Parameters);
