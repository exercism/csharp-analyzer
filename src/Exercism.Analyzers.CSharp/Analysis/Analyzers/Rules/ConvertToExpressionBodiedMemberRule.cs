using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Threading.Tasks;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers.Rules
{
    internal class ConvertToExpressionBodiedMemberRule : Rule
    {
        public override async Task<Diagnostic[]> Verify(CompiledSolution compiledSolution)
        {
            var root = await compiledSolution.ImplementationSyntaxTree.GetRootAsync();

            return root.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Where(MethodCanBeConvertedToExpressionBodiedMember)
                .Select(ToDiagnostic)
                .ToArray();
        }

        private Diagnostic ToDiagnostic(MethodDeclarationSyntax methodSyntax) 
            => new Diagnostic($"Method '{methodSyntax.Identifier.Text}' can be rewritten as an expression-bodied member.", DiagnosticLevel.Information);

        private static bool MethodCanBeConvertedToExpressionBodiedMember(MethodDeclarationSyntax methodSyntax) 
            => methodSyntax.ExpressionBody == null && methodSyntax.Body.Statements.Count == 1;
    }
}