namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class TestSolution
    {
        public string Slug { get; }
        public string Name { get; }
        public string Directory { get; }
        public bool Ignore { get; }

        public TestSolution(string slug, string name, string directory, bool ignore) =>
            (Slug, Name, Directory, Ignore) = (slug, name, directory, ignore);
    }
}