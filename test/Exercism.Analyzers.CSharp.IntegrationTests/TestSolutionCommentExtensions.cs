using System.IO;
using System.Linq;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{   
    internal static class TestSolutionCommentExtensions
    {
        private const string WebsiteCopyDirectory = "website-copy";

        public static string TemplateFilePath(this TestSolutionComment comment)
        {
            var directory = GetRootProjectDirectory();
            if (directory == null)
                return null;

            var pathParts = comment.Comment.Split('.')
                .Prepend("automated-comments")
                .Prepend(WebsiteCopyDirectory)
                .Prepend(directory)
                .ToArray();

            return $"{Path.Combine(pathParts)}.md";
        }

        private static string GetRootProjectDirectory()
        {
            var directory = Directory.GetCurrentDirectory();

            while (directory != null && !Directory.Exists(Path.Combine(directory, WebsiteCopyDirectory)))
                directory = Directory.GetParent(directory)?.FullName;

            return directory;
        }
    }
}