using System.IO;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public class SolutionDownloader
    {
        public async Task<DownloadedSolution> Download(string id)
        {
            var solutionDirectory = await DownloadToDirectory(id);
            
            var solution = await GetSolution(solutionDirectory);
            var projectFile = GetProjectFile(solution, solutionDirectory);
            var implementationFile = GetImplementationFile(solution, solutionDirectory);
            var testsFile = GetTestFileName(solution, solutionDirectory);
            
            return new DownloadedSolution(solution, projectFile, implementationFile, testsFile);
        }

        private static async Task<Solution> GetSolution(DirectoryInfo solutionDirectory)
        {
            using (var textReader = GetSolutionFile(solutionDirectory).OpenText())
            using (var jsonTextReader = new JsonTextReader(textReader))
            {
                var solutionMetadata = await JToken.ReadFromAsync(jsonTextReader);
                var id = solutionMetadata.Value<string>("id");
                var slug = solutionMetadata.Value<string>("exercise");
                
                return new Solution(id, slug);
            }
        }
        
        private static FileInfo GetSolutionFile(DirectoryInfo solutionDirectory) 
            => GetFileInSolutionDirectory(solutionDirectory, ".solution.json");

        private static FileInfo GetProjectFile(Solution solution, DirectoryInfo solutionDirectory) 
            => GetFileInSolutionDirectory(solutionDirectory, $"{solution.Name}.csproj");
        
        private static FileInfo GetImplementationFile(Solution solution, DirectoryInfo solutionDirectory) 
            => GetFileInSolutionDirectory(solutionDirectory, $"{solution.Name}.cs");
        
        private static FileInfo GetTestFileName(Solution solution, DirectoryInfo solutionDirectory) 
            => GetFileInSolutionDirectory(solutionDirectory, $"{solution.Name}Test.cs");

        private static FileInfo GetFileInSolutionDirectory(DirectoryInfo solutionDirectory, string solutionFile) 
            => new FileInfo(Path.Combine(solutionDirectory.FullName, solutionFile));

        protected virtual async Task<DirectoryInfo> DownloadToDirectory(string id)
        {
            // We assume that the exercism CLI is available as a global tool
            var directory = await ProcessRunner.Run("exercism", $"download -u {id}");
            return new DirectoryInfo(directory.Trim());
        }
    }
}