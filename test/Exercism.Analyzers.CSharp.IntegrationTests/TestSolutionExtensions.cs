using System.IO;
using System.Linq;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal static class TestSolutionExtensions
    {
        public static string ReadTestFile(this TestSolution solution, string fileName) =>
            File.ReadAllText(Path.Combine(solution.Directory, fileName));
        
        public static void WriteTestFile(this TestSolution solution, string fileName, string contents) =>
            File.WriteAllText(Path.Combine(solution.Directory, fileName), contents);

        public static void WriteSourceFile(this TestSolution solution, string fileName, string contents) =>
            File.WriteAllText(Path.Combine(solution.ProjectDirectory(), solution.Directory, fileName), contents);

        private static string ProjectDirectory(this TestSolution solution)
        {
            bool IsProjectFile(FileInfo file) => file.Name.EndsWith(".csproj");

            var parent = Directory.GetParent(solution.Directory);

            while (!parent.EnumerateFiles().Any(IsProjectFile))
                parent = parent.Parent;

            return parent.FullName;
        }
    }
}