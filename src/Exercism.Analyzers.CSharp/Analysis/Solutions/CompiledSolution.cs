using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public class CompiledSolution
    {
        public Solution Solution { get; }
        public Compilation Compilation { get; }

        public CompiledSolution(Solution solution, Compilation compilation) 
            => (Solution, Compilation) = (solution, compilation);
    }
}