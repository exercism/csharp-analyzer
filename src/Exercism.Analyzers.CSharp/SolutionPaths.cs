using System.IO;

namespace Exercism.Analyzers.CSharp
{
    internal class SolutionPaths
    {
        public string AnalysisFilePath { get; }
        public string ImplementationFilePath { get; }

        public SolutionPaths(string name, string directory)
        {
            AnalysisFilePath = FilePath(directory, "analysis.json");
            ImplementationFilePath = FilePath(directory, $"{name}.cs");
        }

        private static string FilePath(string directory, string fileName) =>
            Path.GetFullPath(Path.Combine(directory, fileName));
    }
}