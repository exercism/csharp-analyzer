using System.Threading.Tasks;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public static class SolutionCompiler
    {
        public static async Task<CompiledSolution> Compile(LoadedSolution loadedSolution)
        {
            var compilation = await loadedSolution.Project.GetCompilationAsync().ConfigureAwait(false);
            return new CompiledSolution(loadedSolution.Solution, compilation);
        }
    }
}