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
        private readonly SolutionAnalyzer _solutionAnalyzer;

        public Analyzer(
            SolutionDownloader solutionDownloader,
            SolutionLoader solutionLoader,
            SolutionCompiler solutionCompiler,
            SolutionAnalyzer solutionAnalyzer)
        {
            _solutionDownloader = solutionDownloader;
            _solutionLoader = solutionLoader;
            _solutionCompiler = solutionCompiler;
            _solutionAnalyzer = solutionAnalyzer;
        }

        public async Task<AnalysisResult> Analyze(string id)
        {
            var loadedSolution = await LoadSolution(id);
            var compiledSolution = await CompileSolution(loadedSolution);
            return await AnalyzeSolution(compiledSolution);
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

        private async Task<AnalysisResult> AnalyzeSolution(CompiledSolution compiledSolution)
        {
            var analyzedSolution = await _solutionAnalyzer.Analyze(compiledSolution);
            return new AnalysisResult(analyzedSolution.Status, analyzedSolution.Comments);
        }   
    }
}
