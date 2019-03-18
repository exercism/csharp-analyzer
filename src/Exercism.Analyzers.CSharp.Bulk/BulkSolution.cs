namespace Exercism.Analyzers.CSharp.Bulk
{
    public class BulkSolution
    {
        public string Slug { get; }
        public string Directory { get; }

        public BulkSolution(string slug, string directory) =>
            (Slug, Directory) = (slug, directory);
    }
}