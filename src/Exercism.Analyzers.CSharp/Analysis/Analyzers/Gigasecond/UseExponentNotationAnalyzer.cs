using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers.Gigasecond
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class UseExponentNotationAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            id: "EXERCISM0005",
            title: "Use exponent notation",
            messageFormat: "You can write `{0}` as `1e9`.",
            category: "Gigasecond",
            defaultSeverity: DiagnosticSeverity.Info,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context) =>
            context.RegisterSyntaxNodeAction(SyntaxNodeAnalysisContext, SyntaxKind.NumericLiteralExpression);

        private static void SyntaxNodeAnalysisContext(SyntaxNodeAnalysisContext context)
        {
            if (context.SkipAnalysis())
                return;

            var literalExpression = (LiteralExpressionSyntax)context.Node;

            if (IsGigasecond(literalExpression) && !UseExponentNotation(literalExpression))
                context.ReportDiagnostic(Diagnostic.Create(Rule, literalExpression.GetLocation(), literalExpression.ToString()));
        }

        private static bool IsGigasecond(LiteralExpressionSyntax method) =>
            method.Token.IsKind(SyntaxKind.NumericLiteralToken) &&
            method.Token.Value.Equals(1000000000);

        private static bool UseExponentNotation(LiteralExpressionSyntax method) =>
            method.Token.Text.Equals("1e9", StringComparison.OrdinalIgnoreCase);
    }
}