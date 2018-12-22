using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class GigasecondUsesExponentNotationAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            id: "EXERCISM0005",
            title: "Gigasecond does not use exponent notation",
            messageFormat: "You can write `{0}` as `1e9`.",
            category: "Gigasecond",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
            => context.RegisterSyntaxNodeAction(SyntaxNodeAnalysisContext, SyntaxKind.NumericLiteralExpression);

        private static void SyntaxNodeAnalysisContext(SyntaxNodeAnalysisContext context)
        {
            if (context.SkipAnalysis())
                return;

            var method = (LiteralExpressionSyntax)context.Node;

            if (IsGigasecond(method) && !UseExponentNotation(method))
                context.ReportDiagnostic(Diagnostic.Create(Rule, method.GetLocation(), method.ToString()));
        }

        private static bool IsGigasecond(LiteralExpressionSyntax method) 
            => method.Token.IsKind(SyntaxKind.NumericLiteralToken) && 
               method.Token.Value.Equals(1000000000);

        private static bool UseExponentNotation(LiteralExpressionSyntax method)
            => method.Token.Text.Equals("1e9", StringComparison.OrdinalIgnoreCase);
    }
}