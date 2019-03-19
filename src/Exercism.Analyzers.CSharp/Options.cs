using CommandLine;

namespace Exercism.Analyzers.CSharp
{
    internal class Options
    {
        [Value(0, Required = true, HelpText = "The solution's exercise")]
        public string Slug { get; }

        [Value(1, Required = true, HelpText = "The directory containing the solution")]
        public string Directory { get; }

        public Options(string slug, string directory) =>
            (Slug, Directory) = (slug, directory);
    }
}