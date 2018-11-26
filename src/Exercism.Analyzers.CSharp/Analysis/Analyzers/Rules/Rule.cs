using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers.Rules
{
    internal abstract class Rule
    {
        public abstract Task<Diagnostic[]> Verify(CompiledSolution compiledSolution);
    }
}