using CommandLine;

namespace Exercism.Analyzers.CSharp.Bulk
{
    internal class Options
    {
        [Value(0, Required = true, HelpText = "The solutions' exercise")]
        public string Slug { get; }
        
        [Value(1, Required = true, HelpText = "The directory containing the solutions")]
        public string Directory { get; }

        // TODO: convert listing directories to saving analysis run in file
        [Option('l', "list-directories", Required = false, Default = false, HelpText = "Indicates if the solution directories should be listed")]
        public bool ListDirectories { get; }

        public Options(string slug, string directory, bool listDirectories) =>
            (Slug, Directory, ListDirectories) = (slug, directory, listDirectories);
    }
}