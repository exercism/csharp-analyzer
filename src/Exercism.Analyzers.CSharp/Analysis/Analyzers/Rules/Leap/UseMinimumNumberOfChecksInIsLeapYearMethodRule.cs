using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Threading.Tasks;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers.Rules.Leap
{
    internal class UseMinimumNumberOfChecksInIsLeapYearMethodRule : Rule
    {
        private const int MinimalNumberOfChecks = 3;
        
        private const string LeapClassIdentifier = "Leap";
        private const string IsLeapYearMethodIdentifier = "IsLeapYear";
        private const string YearParameterIdentifier = "year";

        public override async Task<Diagnostic[]> Verify(CompiledSolution compiledSolution)
        {
            var root = await compiledSolution.ImplementationSyntaxTree.GetRootAsync();
            
            return root
                    .DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .Where(IsLeapYearMethod)
                    .Where(UsesTooManyChecks)
                    .Select(ToDiagnostic)
                    .ToArray(); 
        }

        private static bool IsLeapYearMethod(MethodDeclarationSyntax methodSyntax)
            => methodSyntax.Identifier.Text == IsLeapYearMethodIdentifier && 
               methodSyntax.Parent is ClassDeclarationSyntax classSyntax && 
               classSyntax.Identifier.Text == LeapClassIdentifier;

        private static bool UsesTooManyChecks(MethodDeclarationSyntax methodSyntax) 
            => methodSyntax
                   .DescendantNodes()
                   .OfType<BinaryExpressionSyntax>()
                   .Count(BinaryExpressionUsesYearParameter) > MinimalNumberOfChecks;

        private static bool BinaryExpressionUsesYearParameter(BinaryExpressionSyntax binaryExpressionSyntax) 
            => ExpressionUsesYearParameter(binaryExpressionSyntax.Left) ||
               ExpressionUsesYearParameter(binaryExpressionSyntax.Right);

        private static bool ExpressionUsesYearParameter(ExpressionSyntax expressionSyntax) 
            => expressionSyntax is IdentifierNameSyntax nameSyntax &&
               nameSyntax.Identifier.Text == YearParameterIdentifier;

        private static Diagnostic ToDiagnostic(MethodDeclarationSyntax methodSyntax)
            => new Diagnostic($"The '{methodSyntax.Identifier.Text}' method uses too many checks.", DiagnosticLevel.Warning);
    }
}