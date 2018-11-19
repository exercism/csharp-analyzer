using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers.Rules
{
    public abstract class Rule
    {
        public abstract Task<Diagnostic[]> Verify(CompiledSolution compiledSolution);
    }
}