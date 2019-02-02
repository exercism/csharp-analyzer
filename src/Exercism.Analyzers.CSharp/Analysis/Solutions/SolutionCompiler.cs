using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public class SolutionCompiler
    {
        private readonly ILogger<SolutionCompiler> _logger;

        public SolutionCompiler(ILogger<SolutionCompiler> logger) => _logger = logger;

        public async Task<CompiledSolution> Compile(LoadedSolution loadedSolution)
        {
            _logger.LogInformation("Compiling solution {ID}",
                loadedSolution.Solution.Id, loadedSolution.Solution.Id);
            
            var compilation = await loadedSolution.Project.GetCompilationAsync();
            
            _logger.LogInformation("Compiled solution {ID}", loadedSolution.Solution.Id);
            
            return new CompiledSolution(loadedSolution.Solution, compilation);
        }
    }
}