namespace Exercism.Analyzers.CSharp;

internal enum CommentType
{
    Essential,
    Actionable,
    Informative,
    Celebratory
}

internal record CommentParameter(string Key, string Value);

internal record Comment(string Text, CommentType Type, params CommentParameter[] Parameters);