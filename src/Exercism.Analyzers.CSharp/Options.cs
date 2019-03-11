using CommandLine;

namespace Exercism.Analyzers.CSharp
{
    public class Options
    {
        [Value(0, Required = true, HelpText = "The slug of the solution's exercise.")]
        public string Exercise { get; }
        
        [Value(1, Required = true, HelpText = "The directory containing the solution.")]
        public string Directory { get; }

        public Options(string exercise, string directory) =>
            (Exercise, Directory) = (exercise, directory);
    }
}