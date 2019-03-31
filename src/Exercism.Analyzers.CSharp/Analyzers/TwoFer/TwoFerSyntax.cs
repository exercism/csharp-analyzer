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
        
        public static bool IsDefaultStringConcatenationExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringConcatenationExpression(
                    TwoFerParameterIdentifierName(twoFerSolution)));

        public static bool UsesStringFormat(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.IsDefaultStringFormatExpression() ||
            twoFerSolution.IsTernaryOperatorStringFormatExpression() ||
            twoFerSolution.IsNullCoalescingStringFormatExpression();

        public static bool IsDefaultStringFormatExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerParameterIdentifierName(twoFerSolution)));

        public static bool IsTernaryOperatorStringFormatExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerTernaryOperatorConditionalExpression(twoFerSolution.InputParameter))) ||
            twoFerSolution.Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerConditionalExpression(
                        TwoFerIsNullOrEmptyInvocationExpression(twoFerSolution),
                        TwoFerParameterIdentifierName(twoFerSolution)))) ||
            twoFerSolution.Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerConditionalExpression(
                        TwoFerIsNullOrWhiteSpaceInvocationExpression(twoFerSolution),
                        TwoFerParameterIdentifierName(twoFerSolution))));

        public static bool IsNullCoalescingStringFormatExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerCoalesceExpression(
                        TwoFerParameterIdentifierName(twoFerSolution))));

        public static bool UsesDefaultInterpolatedStringExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerInterpolatedStringExpression(
                    TwoFerParameterIdentifierName(twoFerSolution)));

        public static bool UsesTernaryOperatorInInterpolatedStringExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerInterpolatedStringExpression(
                    ParenthesizedExpression(
                        TwoFerConditionalExpression(
                            BinaryExpression(
                                SyntaxKind.EqualsExpression,
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

        public static bool IsNullCoalescingStringConcatenationExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerCoalesceExpression(
                            TwoFerParameterIdentifierName(twoFerSolution)))));

        public static bool IsTernaryOperatorStringConcatenationExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerTernaryOperatorConditionalExpression(twoFerSolution.InputParameter)))) ||
            twoFerSolution.Returns(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        ConditionalExpression(
                            TwoFerIsNullOrEmptyInvocationExpression(twoFerSolution),
                            StringLiteralExpression("you"),
                            TwoFerParameterIdentifierName(twoFerSolution))))) ||
            twoFerSolution.Returns(
                        TwoFerStringConcatenationExpression(
                            ParenthesizedExpression(
                                ConditionalExpression(
                                    TwoFerIsNullOrWhiteSpaceInvocationExpression(twoFerSolution),
                                        StringLiteralExpression("you"),
                                    TwoFerParameterIdentifierName(twoFerSolution)))));
        
        public static VariableDeclaratorSyntax AssignedVariable(this MethodDeclarationSyntax nameMethod)
        {
            if (nameMethod == null ||
                nameMethod.Body == null ||
                nameMethod.Body.Statements.Count != 2)
                return null;

            if (!(nameMethod.Body.Statements[1] is ReturnStatementSyntax) ||
                !(nameMethod.Body.Statements[0] is LocalDeclarationStatementSyntax localDeclaration))
                return null;
            
            if (localDeclaration.Declaration.Variables.Count != 1 ||
                !localDeclaration.Declaration.Type.IsEquivalentWhenNormalized(PredefinedType(Token(SyntaxKind.StringKeyword))) &&
                !localDeclaration.Declaration.Type.IsEquivalentWhenNormalized(IdentifierName("var")))
                return null;

            return localDeclaration.Declaration.Variables[0];
        }
    }
}