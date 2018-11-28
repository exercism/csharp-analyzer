using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Threading.Tasks;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers.Rules.Leap
{
    internal class UseMinimumNumberOfBooleanChecksInIsLeapYearRule : Rule
    {
        public override async Task<Diagnostic[]> Verify(CompiledSolution compiledSolution)
        {
            var root = await compiledSolution.ImplementationSyntaxTree.GetRootAsync();

            return root.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Where(IsLeapYearMethod)
                .Where(UsesTooManyChecks)
                .Select(ToDiagnostic)
                .ToArray();
        }

        private Diagnostic ToDiagnostic(MethodDeclarationSyntax methodSyntax)
            => new Diagnostic($"The '{methodSyntax.Identifier.Text}' method uses too many checks.", DiagnosticLevel.Information);

        private static bool IsLeapYearMethod(MethodDeclarationSyntax methodSyntax)
            => methodSyntax.Identifier.Text == "IsLeapYear" && methodSyntax.Parent is ClassDeclarationSyntax classSyntax && classSyntax.Identifier.Text == "Leap";

        private static bool UsesTooManyChecks(MethodDeclarationSyntax methodSyntax)
        {
            return methodSyntax.DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .Where(IsLogicalAndOrLogicalOrExpression)
                .Any();

        }

        private static bool IsLogicalAndOrLogicalOrExpression(BinaryExpressionSyntax binaryExpression) 
            => binaryExpression.Kind() == SyntaxKind.LogicalAndExpression || binaryExpression.Kind() == SyntaxKind.LogicalOrExpression;

        public static bool IsLeapYear(int year)
            => year % 4 == 0 && year % 100 != 0 || year % 100 == 0 && year % 400 == 0;

        public static bool IsLeapYear(int year)
        {
            return year % 4 == 0 && year % 100 != 0 || year % 100 == 0 && year % 400 == 0;
        }
    }
}