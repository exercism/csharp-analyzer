namespace Exercism.Analyzers.CSharp;

internal record SolutionCommentParameter(string Key, string Value);

internal record SolutionComment(string Comment, params SolutionCommentParameter[] Parameters);
