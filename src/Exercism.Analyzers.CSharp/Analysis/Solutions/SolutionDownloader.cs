using System.IO;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions.Helpers;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public class SolutionDownloader
    {
        public async Task<DownloadedSolution> Download(Solution solution)
        {
            var solutionDirectory = await DownloadToDirectory(solution);

            var projectFile = GetProjectFile(solution, solutionDirectory);
            var implementationFile = GetImplementationFile(solution, solutionDirectory);
            var testsFile = GetTestFileName(solution, solutionDirectory);
            
            return new DownloadedSolution(solution, projectFile, implementationFile, testsFile);
        }

        private static FileInfo GetProjectFile(Solution solution, DirectoryInfo solutionDirectory) 
            => GetFileInSolutionDirectory(solutionDirectory, $"{solution.Name}.csproj");
        
        private static FileInfo GetImplementationFile(Solution solution, DirectoryInfo solutionDirectory) 
            => GetFileInSolutionDirectory(solutionDirectory, $"{solution.Name}.cs");
        
        private static FileInfo GetTestFileName(Solution solution, DirectoryInfo solutionDirectory) 
            => GetFileInSolutionDirectory(solutionDirectory, $"{solution.Name}Test.cs");

        private static FileInfo GetFileInSolutionDirectory(DirectoryInfo solutionDirectory, string solutionFile) 
            => new FileInfo(Path.Combine(solutionDirectory.FullName, solutionFile));

        protected virtual async Task<DirectoryInfo> DownloadToDirectory(Solution solution)
        {
            // We assume that the exercism CLI is available as a global tool
            var directory = await ProcessRunner.Run("exercism", $"download -u {solution.Uuid}");
            return new DirectoryInfo(directory.Trim());
        }
    }
}