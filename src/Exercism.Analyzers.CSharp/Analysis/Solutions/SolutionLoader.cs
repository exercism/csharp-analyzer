using Exercism.Analyzers.CSharp.Analysis.Compiling;
using Microsoft.Extensions.Logging;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public class SolutionLoader
    {
        private readonly ILogger<SolutionLoader> _logger;

        public SolutionLoader(ILogger<SolutionLoader> logger) => _logger = logger;

        public LoadedSolution Load(DownloadedSolution downloadedSolution)
        {
            _logger.LogInformation("Loading solution {ID} from {ProjectFile}",
                downloadedSolution.Solution.Id, downloadedSolution.ProjectFile.FullName);
            
            var project = ProjectLoader.LoadFromFile(downloadedSolution.ProjectFile);

            _logger.LogInformation("Loaded solution {ID}", downloadedSolution.Solution.Id);
            
            return new LoadedSolution(downloadedSolution.Solution, project);
        }
    }
}