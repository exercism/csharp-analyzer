using System.IO;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class TestSolution
    {
        public string Slug { get; }
        public string Directory { get; }
        public string DirectoryName { get; }
        public string DirectoryFullPath => Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(TestSolution).Assembly.Location)!, Directory));

        public TestSolution(string slug, string directory) =>
            (Slug, Directory, DirectoryName) = (slug, directory, Path.GetFileName(directory));
    }
}