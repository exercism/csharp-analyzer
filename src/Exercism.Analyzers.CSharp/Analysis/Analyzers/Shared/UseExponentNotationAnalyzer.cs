using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers.Shared
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class UseExponentNotationAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            id: DiagnosticIds.UseExponentNotationAnalyzerRuleId,
            title: "Use exponent notation",
            messageFormat: "You can write `{0}` as `{1}`.",
            category: DiagnosticCategories.LanguageFeature,
            defaultSeverity: DiagnosticSeverity.Info,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterOperationAction(AnalyzeNumericLiteralExpression, OperationKind.Literal);
        }

        private static void AnalyzeNumericLiteralExpression(OperationAnalysisContext context)
        {
            if (context.SkipAnalysis())
                return;

            var literalOperation = (ILiteralOperation)context.Operation;
            if (!IsDouble(literalOperation) && !ImplicitlyConvertedToDouble(literalOperation))
                return;

            var literalExpression = (LiteralExpressionSyntax) literalOperation.Syntax;
            if (!literalExpression.Token.IsKind(SyntaxKind.NumericLiteralToken))
                return;

            if (CouldUseExponentNotation(literalExpression))
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        Rule,
                        literalExpression.GetLocation(),
                        literalExpression.ToString(),
                        SuggestedExpressionUsingExponentNotation(literalExpression.Token)));
        }

        private static bool IsDouble(IOperation literalOperation) =>
            literalOperation.Type.SpecialType.Equals(SpecialType.System_Double);

        private static bool ImplicitlyConvertedToDouble(IOperation literalOperation) =>
            literalOperation.Parent is IConversionOperation conversionOperation &&
            conversionOperation.IsImplicit &&
            conversionOperation.Type.SpecialType.Equals(SpecialType.System_Double);

        private static bool CouldUseExponentNotation(LiteralExpressionSyntax literalExpressionToken) =>
            ValueWarrantsUsingExponentNotation(literalExpressionToken.Token) &&
            !UsesExponentNotation(literalExpressionToken);

        private static bool UsesExponentNotation(LiteralExpressionSyntax literalExpressionToken) =>
            literalExpressionToken.Token.Text.Contains("e", StringComparison.OrdinalIgnoreCase);

        private static bool ValueWarrantsUsingExponentNotation(in SyntaxToken literalExpressionToken) =>
            literalExpressionToken.ValueText.Length > 5 && 
            literalExpressionToken.ValueText.Skip(1).All(c => c == '0');

        private static string SuggestedExpressionUsingExponentNotation(in SyntaxToken literalExpressionToken) =>
            $"{literalExpressionToken.ValueText[0]}e{literalExpressionToken.ValueText.Length - 1}";
    }
}