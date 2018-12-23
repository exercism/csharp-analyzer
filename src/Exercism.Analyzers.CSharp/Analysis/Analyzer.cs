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
            var loadedSolution = await LoadSolution(id).ConfigureAwait(false);
            var compiledSolution = await CompileSolution(loadedSolution).ConfigureAwait(false);
            return await GetSolutionComments(compiledSolution).ConfigureAwait(false);
        }

        private async Task<LoadedSolution> LoadSolution(string id)
        {
            var downloadedSolution = await _solutionDownloader.Download(id).ConfigureAwait(false);
            return _solutionLoader.Load(downloadedSolution);
        }

        private Task<CompiledSolution> CompileSolution(LoadedSolution loadedSolution)
        {
            var analyzers = SolutionAnalyzers.Create(loadedSolution.Solution);
            return _solutionCompiler.Compile(loadedSolution, analyzers);
        }

        private Task<string[]> GetSolutionComments(CompiledSolution compiledSolution) =>
            _solutionComments.GetForSolution(compiledSolution);
    }
}
