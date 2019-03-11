using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Serilog;
using static Exercism.Analyzers.CSharp.Analyzers.LeapSolutions;

namespace Exercism.Analyzers.CSharp.Analyzers
{
    public static class LeapAnalyzer
    {
        public static AnalyzedSolution Analyze(ImplementedSolution implementedSolution)
        {
            Log.Information("Analysing {Exercise} using {Analyzer}", 
                implementedSolution.Solution.Exercise, nameof(LeapAnalyzer));

            if (implementedSolution.IsEquivalentTo(MinimumNumberOfChecks))
                return new AnalyzedSolution(implementedSolution.Solution, SolutionStatus.Approve);

            if (implementedSolution.IsEquivalentTo(UnneededParentheses))
                return new AnalyzedSolution(implementedSolution.Solution, SolutionStatus.Approve);

            if (implementedSolution.IsEquivalentTo(MethodWithBlockBody))
                return new AnalyzedSolution(implementedSolution.Solution, SolutionStatus.Approve, "You could write the method an an expression-bodied member");

            if (implementedSolution.UsesTooManyChecks())
                return new AnalyzedSolution(implementedSolution.Solution, SolutionStatus.ReferToMentor, "Use minimum number of checks");

            return new AnalyzedSolution(implementedSolution.Solution, SolutionStatus.ReferToMentor);
        }

        private static bool UsesTooManyChecks(this ImplementedSolution implementedSolution)
        {
            const int MinimalNumberOfChecks = 3;

            var addMethod = implementedSolution.SyntaxNode
                .GetClass("Leap")
                .GetMethod("IsLeapYear");

            return addMethod
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .Count(BinaryExpressionUsesYearParameter) > MinimalNumberOfChecks;    

            bool BinaryExpressionUsesYearParameter(BinaryExpressionSyntax binaryExpression) =>
                ExpressionUsesYearParameter(binaryExpression.Left) ||
                ExpressionUsesYearParameter(binaryExpression.Right);

            bool ExpressionUsesYearParameter(ExpressionSyntax expression) =>
                expression is IdentifierNameSyntax nameSyntax &&
                nameSyntax.Identifier.Text == "year";
        }        
    }
}