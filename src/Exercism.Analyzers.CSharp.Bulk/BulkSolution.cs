namespace Exercism.Analyzers.CSharp.Bulk
{
    public class BulkSolution
    {
        public string Slug { get; }
        public string Name { get; }
        public string Directory { get; }

        public BulkSolution(string slug, string name, string directory) =>
            (Slug, Name, Directory) = (slug, name, directory);
    }
}