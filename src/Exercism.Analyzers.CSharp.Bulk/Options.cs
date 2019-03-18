namespace Exercism.Analyzers.CSharp.Bulk
{
    internal class Options
    {
        public string Slug { get; }
        public string Directory { get; }

        public Options(string[] args) =>
            (Slug, Directory) = (args[0], args[1]);
    }
}