using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public class CompiledSolution
    {
        public Solution Solution { get; }
        public Compilation Compilation { get; }
        public CompilationWithAnalyzers CompilationWithAnalyzers { get; }

        public CompiledSolution(in Solution solution, CompilationWithAnalyzers compilationWithAnalyzers) =>
            (Solution, Compilation, CompilationWithAnalyzers) = (solution, compilationWithAnalyzers.Compilation, compilationWithAnalyzers);
    }
}