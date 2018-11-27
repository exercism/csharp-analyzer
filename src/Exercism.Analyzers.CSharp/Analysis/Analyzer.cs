using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Analyzers;
using Exercism.Analyzers.CSharp.Analysis.Analyzers.Rules;
using Exercism.Analyzers.CSharp.Analysis.Solutions;

namespace Exercism.Analyzers.CSharp.Analysis
{
    public class Analyzer
    {
        private readonly SolutionDownloader _solutionDownloader;

        public Analyzer(SolutionDownloader solutionDownloader) => _solutionDownloader = solutionDownloader; 
        
        public async Task<Diagnostic[]> Analyze(string id)
        {
            var compiledSolution = await Compile(id);

            return await Analyze(compiledSolution);
        }

        private async Task<CompiledSolution> Compile(string id)
        {
            var downloadedSolution = await _solutionDownloader.Download(id);
            var loadedSolution = SolutionLoader.Load(downloadedSolution);
            return await SolutionCompiler.Compile(loadedSolution);
        }

        private static Task<Diagnostic[]> Analyze(CompiledSolution compiledSolution)
        {
            var solutionAnalyzer = SolutionAnalyzerFactory.Create(compiledSolution.Solution);
            return solutionAnalyzer.Analyze(compiledSolution);
        }
    }
}