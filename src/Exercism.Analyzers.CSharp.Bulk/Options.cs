using CommandLine;

namespace Exercism.Analyzers.CSharp.Bulk
{
    internal class Options
    {
        [Value(0, Required = true, HelpText = "The solutions' exercise")]
        public string Slug { get; }

        [Value(1, Required = true, HelpText = "The directory containing the solutions")]
        public string Directory { get; }

        public Options(string slug, string directory) =>
            (Slug, Directory) = (slug, directory);
    }
}