using System.IO;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class TestSolution
    {
        public string Slug { get; }
        public string Name { get; }
        public string Directory { get; }
        public string DirectoryName { get; }

        public TestSolution(string slug, string name, string directory) =>
            (Slug, Name, Directory, DirectoryName) = (slug, name, directory, Path.GetFileName(directory));
    }
}