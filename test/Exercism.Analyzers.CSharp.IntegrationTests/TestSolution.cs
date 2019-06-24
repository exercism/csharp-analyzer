namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class TestSolution
    {
        public string Slug { get; }
        public string Directory { get; }

        public TestSolution(string slug, string directory) =>
            (Slug, Directory) = (slug, directory);
    }
}