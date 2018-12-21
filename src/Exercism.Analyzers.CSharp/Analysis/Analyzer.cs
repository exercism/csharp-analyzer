using System.Linq;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Analyzers;
using Exercism.Analyzers.CSharp.Analysis.Solutions;

namespace Exercism.Analyzers.CSharp.Analysis
{
    public class Analyzer
    {
        private readonly SolutionDownloader _solutionDownloader;

        public Analyzer(SolutionDownloader solutionDownloader) => _solutionDownloader = solutionDownloader;

        public async Task<string[]> Analyze(string id)
        {
            var loadedSolution = await LoadSolution(id).ConfigureAwait(false);
            var compiledSolution = await CompileSolution(loadedSolution).ConfigureAwait(false);
            return await GetCommentsForSolution(compiledSolution).ConfigureAwait(false);
        }

        private async Task<LoadedSolution> LoadSolution(string id)
        {
            var downloadedSolution = await _solutionDownloader.Download(id).ConfigureAwait(false);
            return SolutionLoader.Load(downloadedSolution);
        }

        private static Task<CompiledSolution> CompileSolution(LoadedSolution loadedSolution)
        {
            var analyzers = SolutionAnalyzers.Create(loadedSolution.Solution);
            return SolutionCompiler.Compile(loadedSolution, analyzers);
        }

        private static async Task<string[]> GetCommentsForSolution(CompiledSolution compiledSolution)
        {
            var diagnostics = await compiledSolution.Compilation.GetAnalyzerDiagnosticsAsync().ConfigureAwait(false);
            return diagnostics.Select(diagnostic => diagnostic.GetMessage()).ToArray();
        }
    }
}
