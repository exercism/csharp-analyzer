using System.IO;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions;

namespace Exercism.Analyzers.CSharp.Tests.Analysis.Solutions
{
    internal class FakeSolutionDownloader : SolutionDownloader
    {
        private static readonly string ExercisesDirectory = Path.Combine("Analysis", "Solutions", "Exercises");
        private static readonly DirectoryInfo FakeSolutionDirectory = new DirectoryInfo("solution");
        
        public string ImplementationFileName { get; set; }

        protected override Task<DirectoryInfo> DownloadToDirectory(Solution solution)
        {
            RemoveOldSolutionFiles();
            CopyNewSolutionFiles(solution);

            return Task.FromResult(FakeSolutionDirectory);
        }

        private static void RemoveOldSolutionFiles()
        {
            if (FakeSolutionDirectory.Exists)
                FakeSolutionDirectory.Delete(recursive: true);

            FakeSolutionDirectory.Create();
        }

        private void CopyNewSolutionFiles(Solution solution)
        {
            CopySolutionFile(solution, ImplementationFileName, GetImplementationFileName(solution));
            CopySolutionFile(solution, GetTestFileName(solution), GetTestFileName(solution));
            CopySolutionFile(solution, GetProjectFileName(solution), GetProjectFileName(solution));
        }

        private static void CopySolutionFile(Solution solution, string solutionFileName, string fakeSolutionFileName) 
            => File.Copy(GetSolutionFilePath(solution, solutionFileName), GetFakeSolutionFilePath(fakeSolutionFileName));

        private static string GetSolutionFilePath(Solution solution, string fileName) 
            => Path.Combine(ExercisesDirectory, solution.Name, fileName);
        
        private static string GetFakeSolutionFilePath(string fileName) => Path.Combine(FakeSolutionDirectory.FullName, fileName);

        private static string GetImplementationFileName(Solution solution) => $"{solution.Name}.cs";

        private static string GetTestFileName(Solution solution) => $"{solution.Name}Test.cs";

        private static string GetProjectFileName(Solution solution) => $"{solution.Name}.csproj";
    }
}