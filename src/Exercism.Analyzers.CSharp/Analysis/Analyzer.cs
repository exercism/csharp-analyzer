using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Analyzers;
using Exercism.Analyzers.CSharp.Analysis.Solutions;

namespace Exercism.Analyzers.CSharp.Analysis
{
    public class Analyzer
    {
        private readonly SolutionDownloader _solutionDownloader;
        private readonly SolutionCompiler _solutionCompiler;
        private readonly SolutionLoader _solutionLoader;
        private readonly SolutionComments _solutionComments;

        public Analyzer(
            SolutionDownloader solutionDownloader,
            SolutionLoader solutionLoader,
            SolutionCompiler solutionCompiler,
            SolutionComments solutionComments)
        {
            _solutionDownloader = solutionDownloader;
            _solutionLoader = solutionLoader;
            _solutionCompiler = solutionCompiler;
            _solutionComments = solutionComments;
        }

        public async Task<string[]> Analyze(string id)
        {
            var loadedSolution = await LoadSolution(id);
            var compiledSolution = await CompileSolution(loadedSolution);
            return await GetSolutionComments(compiledSolution);
        }

        private async Task<LoadedSolution> LoadSolution(string id)
        {
            var downloadedSolution = await _solutionDownloader.Download(id);
            return _solutionLoader.Load(downloadedSolution);
        }

        private async Task<CompiledSolution> CompileSolution(LoadedSolution loadedSolution)
        {
            var analyzers = SolutionAnalyzers.Create(loadedSolution.Solution);
            return await _solutionCompiler.Compile(loadedSolution, analyzers);
        }

        private async Task<string[]> GetSolutionComments(CompiledSolution compiledSolution) =>
            await _solutionComments.GetForSolution(compiledSolution);
    }
}
