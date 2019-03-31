using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal static class TwoFerSyntax
    {
        public static bool AssignsToParameter(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.AssignsToParameter(twoFerSolution.InputParameter);

        public static bool NoDefaultValue(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.ParameterList.Parameters.All(parameter => parameter.Default == null);

        public static bool UsesInvalidDefaultValue(this TwoFerSolution twoFerSolution) =>
            !twoFerSolution.InputParameter.Default.Value.IsEquivalentWhenNormalized(NullLiteralExpression()) &&
            !twoFerSolution.InputParameter.Default.Value.IsEquivalentWhenNormalized(StringLiteralExpression("you"));

        public static bool UsesStringConcatenation(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.IsDefaultStringConcatenationExpression() ||
            twoFerSolution.IsNullCoalescingStringConcatenationExpression() ||
            twoFerSolution.IsTernaryOperatorStringConcatenationExpression();

        private static bool IsDefaultStringConcatenationExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringConcatenationExpression(
                    TwoFerParameterIdentifierName(twoFerSolution)));

        private static bool IsNullCoalescingStringConcatenationExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerCoalesceExpression(
                            TwoFerParameterIdentifierName(twoFerSolution)))));

        private static bool IsTernaryOperatorStringConcatenationExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.IsNullCheckTernaryOperatorStringConcatenationExpression() ||
            twoFerSolution.IsIsNullOrEmptyTernaryOperatorStringConcatenationExpression() ||
            twoFerSolution.IsIsNullOrWhiteSpaceTernaryOperatorStringConcatenationExpression();

        private static bool IsIsNullOrWhiteSpaceTernaryOperatorStringConcatenationExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerConditionalExpression(
                            TwoFerIsNullOrWhiteSpaceInvocationExpression(twoFerSolution),
                            TwoFerParameterIdentifierName(twoFerSolution)))));

        private static bool IsIsNullOrEmptyTernaryOperatorStringConcatenationExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerConditionalExpression(
                            TwoFerIsNullOrEmptyInvocationExpression(twoFerSolution),
                            TwoFerParameterIdentifierName(twoFerSolution)))));

        private static bool IsNullCheckTernaryOperatorStringConcatenationExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerTernaryOperatorConditionalExpression(twoFerSolution.InputParameter))));

        public static bool UsesStringFormat(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.IsDefaultStringFormatExpression() ||
            twoFerSolution.IsNullCoalescingStringFormatExpression() ||
            twoFerSolution.IsTernaryOperatorStringFormatExpression();

        private static bool IsDefaultStringFormatExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerParameterIdentifierName(twoFerSolution)));

        private static bool IsNullCoalescingStringFormatExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerCoalesceExpression(
                        TwoFerParameterIdentifierName(twoFerSolution))));

        private static bool IsTernaryOperatorStringFormatExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.IsNullCheckTernaryOperatorStringFormatExpression() ||
            twoFerSolution.IsIsNullOrEmptyTernaryOperatorStringFormatExpression() ||
            twoFerSolution.IsIsNullOrWhiteSpaceTernaryOperatorStringFormatExpression();

        private static bool IsIsNullOrWhiteSpaceTernaryOperatorStringFormatExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerConditionalExpression(
                        TwoFerIsNullOrWhiteSpaceInvocationExpression(twoFerSolution),
                        TwoFerParameterIdentifierName(twoFerSolution))));

        private static bool IsIsNullOrEmptyTernaryOperatorStringFormatExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerConditionalExpression(
                        TwoFerIsNullOrEmptyInvocationExpression(twoFerSolution),
                        TwoFerParameterIdentifierName(twoFerSolution))));

        private static bool IsNullCheckTernaryOperatorStringFormatExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerTernaryOperatorConditionalExpression(twoFerSolution.InputParameter)));
        
        public static bool UsesDefaultInterpolatedStringExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerInterpolatedStringExpression(
                    TwoFerParameterIdentifierName(twoFerSolution)));

        public static bool UsesTernaryOperatorInInterpolatedStringExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerInterpolatedStringExpression(
                    ParenthesizedExpression(
                        TwoFerConditionalExpression(
                            EqualsExpression(
                                TwoFerParameterIdentifierName(twoFerSolution),
                                NullLiteralExpression()),
                            TwoFerParameterIdentifierName(twoFerSolution)))));

        public static bool UsesNullCoalescingInInterpolatedStringExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerInterpolatedStringExpression(
                    TwoFerCoalesceExpression(
                        TwoFerParameterIdentifierName(twoFerSolution))));

        public static bool UsesIsNullOrEmptyInInterpolatedStringExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerInterpolatedStringExpression(
                    ParenthesizedExpression(
                        TwoFerConditionalExpression(
                            TwoFerIsNullOrEmptyInvocationExpression(twoFerSolution),
                            TwoFerParameterIdentifierName(twoFerSolution)))));

        public static bool UsesIsNullOrWhiteSpaceInInterpolatedStringExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerInterpolatedStringExpression(
                    ParenthesizedExpression(
                        TwoFerConditionalExpression(
                            TwoFerIsNullOrWhiteSpaceInvocationExpression(twoFerSolution),
                            TwoFerParameterIdentifierName(twoFerSolution)))));
    }
}