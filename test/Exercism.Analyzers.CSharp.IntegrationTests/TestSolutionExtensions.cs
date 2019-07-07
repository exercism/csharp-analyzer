using System.IO;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal static class TestSolutionExtensions
    {
        public static string ReadFile(this TestSolution solution, string filePath) =>
            File.ReadAllText(solution.FilePath(filePath));

        private static string FilePath(this TestSolution solution, string filePath) =>
            Path.Combine(solution.Directory, filePath);
    }
}