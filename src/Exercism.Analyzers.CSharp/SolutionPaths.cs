using System.IO;

namespace Exercism.Analyzers.CSharp
{
    public class SolutionPaths
    {
        public string Directory { get; }
        public string SolutionFilePath { get; }
        public string AnalysisFilePath { get; }
        public string ImplementationFilePath { get; }
        
        public SolutionPaths(string name, string directory)
        {
            Directory = directory;
            SolutionFilePath = FilePath(directory, ".solution.json");
            AnalysisFilePath = FilePath(directory, "analysis.json");
            ImplementationFilePath = FilePath(directory, $"{name}.cs");
        }

        private static string FilePath(string directory, string fileName) =>
            Path.GetFullPath(Path.Combine(directory, fileName));
    }
}