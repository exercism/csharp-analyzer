using System.IO;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions;

namespace Exercism.Analyzers.CSharp.Tests.Analysis.Solutions
{
    internal class FakeSolutionDownloader : SolutionDownloader
    {
        private static readonly string SourceExercisesDirectory = Path.Combine("Analysis", "Solutions", "Exercises");

        private DirectoryInfo _fakeSolutionDirectory;
        private string _implementationFileName;

        public void Configure(Solution solution, string implementationFileSuffix)
        {
            _fakeSolutionDirectory = GetFakeSolutionDirectory(implementationFileSuffix);
            _implementationFileName = GetImplementationFileName(solution, implementationFileSuffix);
        }

        private static DirectoryInfo GetFakeSolutionDirectory(string implementationFileSuffix) 
            => new DirectoryInfo(Path.Combine(SourceExercisesDirectory, implementationFileSuffix));

        private static string GetImplementationFileName(Solution solution, string implementationFileSuffix) 
            => $"{solution.Name}{implementationFileSuffix}.cs";

        protected override Task<DirectoryInfo> DownloadToDirectory(Solution solution)
        {
            CreateFakeSolution(solution);

            return Task.FromResult(_fakeSolutionDirectory);
        }

        private void CreateFakeSolution(Solution solution)
        {
            CreateFakeSolutionDirectory();
            CopySolutionFile(solution, _implementationFileName, GetImplementationFileName(solution));
            CopySolutionFile(solution, GetTestFileName(solution), GetTestFileName(solution));
            CopySolutionFile(solution, GetProjectFileName(solution), GetProjectFileName(solution));
        }

        private void CreateFakeSolutionDirectory()
        {
            if (_fakeSolutionDirectory.Exists)
                _fakeSolutionDirectory.Delete(recursive: true);

            _fakeSolutionDirectory.Create();
        }

        private void CopySolutionFile(Solution solution, string sourceSolutionFileName, string fakeSolutionFileName) 
            => File.Copy(
                GetSourceSolutionFilePath(solution, sourceSolutionFileName),
                GetFakeSolutionFilePath(fakeSolutionFileName));

        private static string GetSourceSolutionFilePath(Solution solution, string fileName) 
            => Path.Combine(SourceExercisesDirectory, solution.Name, fileName);
        
        private string GetFakeSolutionFilePath(string fileName) 
            => Path.Combine(_fakeSolutionDirectory.FullName, fileName);

        private static string GetImplementationFileName(Solution solution) => $"{solution.Name}.cs";

        private static string GetTestFileName(Solution solution) => $"{solution.Name}Test.cs";

        private static string GetProjectFileName(Solution solution) => $"{solution.Name}.csproj";
    }
}