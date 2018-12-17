using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class LeapUsesMinimumNumberOfChecksAnalyzer : DiagnosticAnalyzer
    {
        private const int MinimalNumberOfChecks = 3;
        private const string LeapClassIdentifier = "Leap";
        private const string IsLeapYearMethodIdentifier = "IsLeapYear";
        private const string YearParameterIdentifier = "year";
        
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            id: "EXERCISM0003",
            title: "Leap uses minimum number of checks",
            messageFormat: $"The '{IsLeapYearMethodIdentifier}' method uses too many checks.",
            category: "Leap",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
            => context.RegisterSyntaxNodeAction(SyntaxNodeAnalysisContext, SyntaxKind.MethodDeclaration);

        private static void SyntaxNodeAnalysisContext(SyntaxNodeAnalysisContext context)
        {
            var method = (MethodDeclarationSyntax)context.Node;

            if (IsLeapYearMethod(method) && UsesTooManyChecks(method))
                context.ReportDiagnostic(Diagnostic.Create(Rule, method.GetLocation()));
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
    }
}