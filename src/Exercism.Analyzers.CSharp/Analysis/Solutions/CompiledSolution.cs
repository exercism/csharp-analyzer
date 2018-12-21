using Microsoft.CodeAnalysis.Diagnostics;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public class CompiledSolution
    {
        public Solution Solution { get; }
        public CompilationWithAnalyzers Compilation { get; }

        public CompiledSolution(Solution solution, CompilationWithAnalyzers compilation) 
            => (Solution, Compilation) = (solution, compilation);
    }
}