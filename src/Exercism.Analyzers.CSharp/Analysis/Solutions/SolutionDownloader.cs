using System.IO;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.CommandLine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public class SolutionDownloader
    {
        private readonly ExercismCommandLineInterface _exercismCommandLineInterface;

        public SolutionDownloader(ExercismCommandLineInterface exercismCommandLineInterface) 
            => _exercismCommandLineInterface = exercismCommandLineInterface;

        public async Task<DownloadedSolution> Download(string id)
        {
            var solutionDirectory = await DownloadToDirectory(id);
            
            var solution = await GetSolution(solutionDirectory);
            var projectFile = GetProjectFile(solution, solutionDirectory);
            var implementationFile = GetImplementationFile(solution, solutionDirectory);
            var testsFile = GetTestFileName(solution, solutionDirectory);
            
            return new DownloadedSolution(solution, projectFile, implementationFile, testsFile);
        }
        
        private Task<DirectoryInfo> DownloadToDirectory(string id) => _exercismCommandLineInterface.Download(id);

        private static async Task<Solution> GetSolution(DirectoryInfo solutionDirectory)
        {
            using (var textReader = GetMetadataFile(solutionDirectory).OpenText())
            using (var jsonTextReader = new JsonTextReader(textReader))
            {
                var solutionMetadata = await JToken.ReadFromAsync(jsonTextReader);
                var id = solutionMetadata.Value<string>("id");
                var slug = solutionMetadata.Value<string>("exercise");
                
                return new Solution(id, slug);
            }
        }
        
        private static FileInfo GetMetadataFile(DirectoryInfo solutionDirectory) 
            => GetFileInSolutionDirectory(solutionDirectory, Path.Combine(".exercism", "metadata.json"));

        private static FileInfo GetProjectFile(Solution solution, DirectoryInfo solutionDirectory) 
            => GetFileInSolutionDirectory(solutionDirectory, $"{solution.Name}.csproj");
        
        private static FileInfo GetImplementationFile(Solution solution, DirectoryInfo solutionDirectory) 
            => GetFileInSolutionDirectory(solutionDirectory, $"{solution.Name}.cs");
        
        private static FileInfo GetTestFileName(Solution solution, DirectoryInfo solutionDirectory) 
            => GetFileInSolutionDirectory(solutionDirectory, $"{solution.Name}Test.cs");

        private static FileInfo GetFileInSolutionDirectory(DirectoryInfo solutionDirectory, string solutionFile) 
            => new FileInfo(Path.Combine(solutionDirectory.FullName, solutionFile));
    }
}