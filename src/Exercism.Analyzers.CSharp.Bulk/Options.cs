using System.Linq;

namespace Exercism.Analyzers.CSharp.Bulk
{
    internal class Options
    {
        public string Slug { get; }
        public string Directory { get; }
        public bool ListDirectories { get; }

        public Options(string[] args) =>
            (Slug, Directory, ListDirectories) = (args[0], args[1], args.Contains("--list-directories"));
    }
}