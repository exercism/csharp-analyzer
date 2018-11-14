using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    public abstract class SolutionAnalyzer
    {
        public abstract Task<Diagnostic[]> Analyze(CompiledSolution compiledSolution);
    }
}