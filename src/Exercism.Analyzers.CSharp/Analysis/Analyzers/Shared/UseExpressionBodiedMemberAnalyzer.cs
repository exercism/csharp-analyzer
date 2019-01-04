using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers.Shared
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class UseExpressionBodiedMemberAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            id: "EXERCISM0004",
            title: "Use an expression-bodied member",
            messageFormat: "The '{0}' method can be rewritten as an expression-bodied member.",
            category: "Shared",
            defaultSeverity: DiagnosticSeverity.Info,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context) =>
            context.RegisterSyntaxNodeAction(SyntaxNodeAnalysisContext, SyntaxKind.MethodDeclaration);

        private static void SyntaxNodeAnalysisContext(SyntaxNodeAnalysisContext context)
        {
            if (context.SkipAnalysis())
                return;
            
            var method = (MethodDeclarationSyntax)context.Node;

            // TODO: add base class to help verify if the file being processed is not the test file
            
            if (MethodCanBeConvertedToExpressionBodiedMember(method))
                context.ReportDiagnostic(Diagnostic.Create(Rule, method.GetLocation(), method.Identifier.ValueText));
        }

        private static bool MethodCanBeConvertedToExpressionBodiedMember(MethodDeclarationSyntax methodSyntax) =>
            methodSyntax.ExpressionBody == null && methodSyntax.Body.Statements.Count == 1;
    }
}