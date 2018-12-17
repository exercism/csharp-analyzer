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
            var compiledSolution = await Compile(id).ConfigureAwait(false);
            return await GetComments(compiledSolution).ConfigureAwait(false);
        }

        private async Task<CompiledSolution> Compile(string id)
        {
            var downloadedSolution = await _solutionDownloader.Download(id).ConfigureAwait(false);
            var loadedSolution = SolutionLoader.Load(downloadedSolution);
            return await SolutionCompiler.Compile(loadedSolution).ConfigureAwait(false);
        }

        private static async Task<string[]> GetComments(CompiledSolution compiledSolution)
        {
            var diagnostics = await SolutionAnalyzer.Analyze(compiledSolution).ConfigureAwait(false);
            return diagnostics.Select(diagnostic => diagnostic.GetMessage()).ToArray();
        }
    }
}
