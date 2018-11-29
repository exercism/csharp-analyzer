using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Compilation;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public static class SolutionCompiler
    {
        public static async Task<CompiledSolution> Compile(LoadedSolution loadedSolution)
        {   
            var implementationSyntaxTree = await loadedSolution.ImplementationFile.GetSyntaxTreeAsync().ConfigureAwait(false);
            var testsSyntaxTree = await loadedSolution.TestsFile.GetSyntaxTreeAsync().ConfigureAwait(false);
            var testsWithoutSkipPropertySyntaxTree = testsSyntaxTree.WithRewrittenRoot(new RemoveAttributeArgumentAttributeRewriter("Skip", "Fact"));
            
            var compilation = await loadedSolution.Project.GetCompilationAsync().ConfigureAwait(false);
            var updatedCompilation = compilation.ReplaceSyntaxTree(testsSyntaxTree, testsWithoutSkipPropertySyntaxTree);
            
            return new CompiledSolution(loadedSolution.Solution, updatedCompilation, implementationSyntaxTree, testsWithoutSkipPropertySyntaxTree);
        }
    }
}