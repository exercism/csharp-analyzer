using CommandLine;

namespace Exercism.Analyzers.CSharp
{
    public class Options
    {
        [Value(0, Required = true, HelpText = "The slug of the solution's exercise.")]
        public string Slug { get; }
        
        [Value(1, Required = true, HelpText = "The directory containing the solution.")]
        public string Directory { get; }

        public Options(string slug, string directory) =>
            (Slug, Directory) = (slug, directory);
    }
}