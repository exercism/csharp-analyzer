using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers.Shared
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class UseDigitSeparatorAnalyzer : DiagnosticAnalyzer
    {
        private enum NumericLiteralType
        {
            Decimal,
            Hexadecimal,
            Binary
        }

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            id: "EXERCISM0006",
            title: "Use digit separator",
            messageFormat: "You can write `{0}` as `{1}`.",
            category: "Shared",
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

            if (!literalExpression.Token.IsKind(SyntaxKind.NumericLiteralToken))
                return;

            if (CouldUseDigitSeparator(literalExpression.Token))
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        Rule,
                        literalExpression.GetLocation(),
                        literalExpression.ToString(),
                        SuggestedExpressionUsingDigitSeparator(literalExpression.Token)));
        }

        private static bool CouldUseDigitSeparator(SyntaxToken literalExpressionToken) =>
            ValueIsInteger(literalExpressionToken) &&
            ValueNotDefinedUsingDigitSeparator(literalExpressionToken) &&
            ValueWarrantsUsingDigitSeparator(literalExpressionToken);

        private static bool ValueIsInteger(in SyntaxToken literalExpressionToken) =>
            literalExpressionToken.Value is int;

        private static bool ValueNotDefinedUsingDigitSeparator(SyntaxToken methodToken) =>
            !methodToken.Text.Contains("_");

        private static bool ValueWarrantsUsingDigitSeparator(SyntaxToken literalExpressionToken)
        {
            var value = (int) literalExpressionToken.Value;

            if (IsBinaryNumber(literalExpressionToken))
                return value > 0b1111;

            if (IsHexadecimalNumber(literalExpressionToken))
                return value > 0xFF;

            return value > 99999;
        }

        private static bool IsBinaryNumber(SyntaxToken methodToken) =>
            methodToken.Text.StartsWith("0b", StringComparison.OrdinalIgnoreCase);

        private static bool IsHexadecimalNumber(SyntaxToken methodToken) =>
            methodToken.Text.StartsWith("0x", StringComparison.OrdinalIgnoreCase);

        private static string SuggestedExpressionUsingDigitSeparator(SyntaxToken literalExpressionToken)
        {
            var text = literalExpressionToken.Text.AsSpan();

            if (IsBinaryNumber(literalExpressionToken))
                return literalExpressionToken.Text.Substring(0, 2) + AddDigitSeparator(text.Slice(2), 4);

            if (IsHexadecimalNumber(literalExpressionToken))
                return literalExpressionToken.Text.Substring(0, 2) + AddDigitSeparator(text.Slice(2), 2);

            return AddDigitSeparator(text, 3);
        }

        private static string AddDigitSeparator(ReadOnlySpan<char> literalExpression, int separatorWidth)
        {
            var literalExpressionWithSeparators = new Stack<char>();

            for (var i = 0; i < literalExpression.Length; i++)
            {
                literalExpressionWithSeparators.Push(literalExpression[literalExpression.Length - 1 - i]);
                if (i > 0 && i < literalExpression.Length - 1 && (i + 1) % separatorWidth == 0)
                    literalExpressionWithSeparators.Push('_');
            }

            return new string(literalExpressionWithSeparators.ToArray());
        }
    }
}