using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal static class TestSolutionMarkdownGenerator
    {
        private const string CommentSeparator = "\n\n";

        public static string Generate(IEnumerable<TestSolutionComment> comments) =>
            string.Join(CommentSeparator, comments.Select(Generate));

        private static string Generate(TestSolutionComment comment, int index) =>
            $"{CommentHeader(index)}\n{CommentText(comment)}";

        private static string CommentHeader(int index) =>
            $"[COMMENT #{index + 1}]";

        private static string CommentText(TestSolutionComment comment) =>
            comment.Parameters.Aggregate(comment.ReadTemplate(), ReplaceParameterPlaceholder).Trim();

        private static string ReplaceParameterPlaceholder(string current, TestSolutionCommentParameter parameter) =>
            current.Replace($"%{{{parameter.Key}}}", parameter.Value);

        private static string ReadTemplate(this TestSolutionComment comment) =>
            File.ReadAllText(comment.TemplateFilePath());
    }
}