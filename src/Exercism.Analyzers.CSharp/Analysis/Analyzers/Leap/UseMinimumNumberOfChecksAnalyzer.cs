using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers.Leap
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class UseMinimumNumberOfChecksAnalyzer : DiagnosticAnalyzer
    {
        private const int MinimalNumberOfChecks = 3;
        private const string LeapClassIdentifier = "Leap";
        private const string IsLeapYearMethodIdentifier = "IsLeapYear";
        private const string YearParameterIdentifier = "year";
        
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            id: DiagnosticIds.UseMinimumNumberOfChecksAnalyzerRuleId,
            title: "Leap does not use minimum number of checks",
            messageFormat: $"The '{IsLeapYearMethodIdentifier}' method uses too many checks.",
            category: DiagnosticCategories.Optimization,
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(SyntaxNodeAnalysisContext, SyntaxKind.MethodDeclaration);
        }

        private static void SyntaxNodeAnalysisContext(SyntaxNodeAnalysisContext context)
        {
            if (context.SkipAnalysis())
                return;
            
            var method = (MethodDeclarationSyntax)context.Node;

            if (IsLeapYearMethod(method) && UsesTooManyChecks(method))
                context.ReportDiagnostic(Diagnostic.Create(Rule, method.GetLocation()));
        }

        private static bool IsLeapYearMethod(MethodDeclarationSyntax method) =>
            method.Identifier.Text == IsLeapYearMethodIdentifier && 
            method.Parent is ClassDeclarationSyntax classSyntax && 
            classSyntax.Identifier.Text == LeapClassIdentifier;

        private static bool UsesTooManyChecks(MethodDeclarationSyntax method) =>
            method
               .DescendantNodes()
               .OfType<BinaryExpressionSyntax>()
               .Count(BinaryExpressionUsesYearParameter) > MinimalNumberOfChecks;

        private static bool BinaryExpressionUsesYearParameter(BinaryExpressionSyntax binaryExpression) =>
            ExpressionUsesYearParameter(binaryExpression.Left) ||
            ExpressionUsesYearParameter(binaryExpression.Right);

        private static bool ExpressionUsesYearParameter(ExpressionSyntax expression) =>
            expression is IdentifierNameSyntax nameSyntax &&
            nameSyntax.Identifier.Text == YearParameterIdentifier;
    }
}