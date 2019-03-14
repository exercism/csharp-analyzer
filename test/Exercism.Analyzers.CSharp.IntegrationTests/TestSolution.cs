namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class TestSolution
    {   
        public string Slug { get; }
        public string Name { get; }
        public string Directory { get; }

        public TestSolution(string slug, string name, string directory) =>
            (Slug, Name, Directory) = (slug, name, directory);
    }
}