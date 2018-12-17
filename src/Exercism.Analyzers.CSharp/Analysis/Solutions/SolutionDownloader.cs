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
            var solutionDirectory = await DownloadToDirectory(id).ConfigureAwait(false);
            
            var solution = await GetSolution(solutionDirectory).ConfigureAwait(false);
            var projectFile = GetProjectFile(solution, solutionDirectory);
            
            return new DownloadedSolution(solution, projectFile);
        }
        
        private Task<DirectoryInfo> DownloadToDirectory(string id) => _exercismCommandLineInterface.Download(id);

        private static async Task<Solution> GetSolution(DirectoryInfo solutionDirectory)
        {
            using (var textReader = GetMetadataFile(solutionDirectory).OpenText())
            using (var jsonTextReader = new JsonTextReader(textReader))
            {
                var solutionMetadata = await JToken.ReadFromAsync(jsonTextReader).ConfigureAwait(false);
                var id = solutionMetadata.Value<string>("id");
                var slug = solutionMetadata.Value<string>("exercise");
                
                return new Solution(id, slug);
            }
        }
        
        private static FileInfo GetMetadataFile(FileSystemInfo solutionDirectory) 
            => GetFileInSolutionDirectory(solutionDirectory, Path.Combine(".exercism", "metadata.json"));

        private static FileInfo GetProjectFile(Solution solution, FileSystemInfo solutionDirectory) 
            => GetFileInSolutionDirectory(solutionDirectory, $"{solution.Name}.csproj");

        private static FileInfo GetFileInSolutionDirectory(FileSystemInfo solutionDirectory, string solutionFile) 
            => new FileInfo(Path.Combine(solutionDirectory.FullName, solutionFile));
    }
}