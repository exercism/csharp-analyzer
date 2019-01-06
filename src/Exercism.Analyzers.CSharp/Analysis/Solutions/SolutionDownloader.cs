using System.IO;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.CommandLine;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public class SolutionDownloader
    {
        private readonly ExercismCommandLineInterface _exercismCommandLineInterface;
        private readonly ILogger<SolutionDownloader> _logger;

        public SolutionDownloader(ExercismCommandLineInterface exercismCommandLineInterface, ILogger<SolutionDownloader> logger) =>
            (_exercismCommandLineInterface, _logger) = (exercismCommandLineInterface, logger);

        public async Task<DownloadedSolution> Download(string id)
        {
            _logger.LogInformation("Downloading solution {ID}", id);
            
            var solutionDirectory = await DownloadToDirectory(id).ConfigureAwait(false);
            
            _logger.LogInformation("Downloaded solution {ID} to {SolutionDirectory}", 
                id, solutionDirectory.FullName);
            
            var solution = await GetSolution(solutionDirectory).ConfigureAwait(false);
            var projectFile = GetProjectFile(solution, solutionDirectory);
            
            return new DownloadedSolution(solution, projectFile);
        }
        
        private async Task<DirectoryInfo> DownloadToDirectory(string id) =>
            await _exercismCommandLineInterface.Download(id);

        private static async Task<Solution> GetSolution(DirectoryInfo solutionDirectory)
        {
            using (var textReader = GetMetadataFile(solutionDirectory).OpenText())
            using (var jsonTextReader = new JsonTextReader(textReader))
            {
                var solutionMetadata = await JToken.ReadFromAsync(jsonTextReader).ConfigureAwait(false);
                var id = solutionMetadata.Value<string>("id");
                var slug = solutionMetadata.Value<string>("exercise");

                var exercise = new Exercise(slug);
                return new Solution(id, exercise);
            }
        }
        
        private static FileInfo GetMetadataFile(FileSystemInfo solutionDirectory) =>
            GetFileInSolutionDirectory(solutionDirectory, Path.Combine(".exercism", "metadata.json"));

        private static FileInfo GetProjectFile(in Solution solution, FileSystemInfo solutionDirectory) =>
            GetFileInSolutionDirectory(solutionDirectory, $"{solution.Exercise.Name}.csproj");

        private static FileInfo GetFileInSolutionDirectory(FileSystemInfo solutionDirectory, string solutionFile) =>
            new FileInfo(Path.Combine(solutionDirectory.FullName, solutionFile));
    }
}