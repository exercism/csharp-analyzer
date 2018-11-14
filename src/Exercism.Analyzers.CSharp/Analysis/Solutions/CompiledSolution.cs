using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public class CompiledSolution
    {
        public Solution Solution { get; }

        public Compilation Compilation { get; }
        public SyntaxTree ImplementationSyntaxTree { get; }
        public SyntaxTree TestsSyntaxTree { get; }

        public CompiledSolution(Solution solution, Compilation compilation, SyntaxTree implementationSyntax, SyntaxTree testsSyntax) 
            => (Solution, Compilation, ImplementationSyntaxTree, TestsSyntaxTree) = (solution, compilation, implementationSyntax, testsSyntax);
    }
}