using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Roslyn;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public static class SolutionCompiler
    {
        public static async Task<CompiledSolution> Compile(LoadedSolution loadedSolution)
        {   
            var implementationSyntaxTree = await loadedSolution.ImplementationFile.GetSyntaxTreeAsync();
            var testsSyntaxTree = await loadedSolution.TestsFile.GetSyntaxTreeAsync();
            var testsWithoutSkipPropertySyntaxTree = testsSyntaxTree.WithRewrittenRoot(new RemoveAttributeArgumentAttributeRewriter("Skip", "Fact"));
            
            var compilation = await loadedSolution.Project.GetCompilationAsync();
            var updatedCompilation = compilation.ReplaceSyntaxTree(testsSyntaxTree, testsWithoutSkipPropertySyntaxTree);
            
            return new CompiledSolution(loadedSolution.Solution, updatedCompilation, implementationSyntaxTree, testsWithoutSkipPropertySyntaxTree);
        }
    }
}