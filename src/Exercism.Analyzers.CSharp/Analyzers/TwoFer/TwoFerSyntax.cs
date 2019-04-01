using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal static class TwoFerSyntax
    {
        public static bool MissingNameMethod(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod == null;

        public static bool InvalidNameMethod(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.ParameterList.Parameters.Count != 1 ||
            !twoFerSolution.InputParameter.Type.IsEquivalentWhenNormalized(
                PredefinedType(Token(SyntaxKind.StringKeyword)));

        public static bool UsesOverloads(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.TwoFerClass.GetMethods("Name").Count() > 1;

        public static bool UsesDuplicateString(this TwoFerSolution twoFerSolution)
        {
            var literalExpressionCount = twoFerSolution.NameMethod
                .DescendantNodes<LiteralExpressionSyntax>()
                .Count(literalExpression => literalExpression.Token.ValueText.Contains("One for"));

            var interpolatedStringTextCount = twoFerSolution.NameMethod
                .DescendantNodes<InterpolatedStringTextSyntax>()
                .Count(interpolatedStringText => interpolatedStringText.TextToken.ValueText.Contains("One for"));

            return literalExpressionCount + interpolatedStringTextCount > 1;
        }

        public static bool UsesStringJoin(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.InvokesMethod(StringMemberAccessExpression(IdentifierName("Join")));

        public static bool UsesStringConcat(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.InvokesMethod(StringMemberAccessExpression(IdentifierName("Concat")));

        public static bool UsesStringReplace(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.InvokesMethod(IdentifierName("Replace"));

        public static bool AssignsToParameter(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.AssignsToParameter(twoFerSolution.InputParameter);

        public static bool NoDefaultValue(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.ParameterList.Parameters.All(parameter => parameter.Default == null);

        public static bool UsesSingleLine(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.SingleLine();

        public static bool UsesExpressionBody(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.IsExpressionBody();

        public static bool UsesInvalidDefaultValue(this TwoFerSolution twoFerSolution) =>
            !twoFerSolution.InputParameter.Default.Value.IsEquivalentWhenNormalized(NullLiteralExpression()) &&
            !twoFerSolution.InputParameter.Default.Value.IsEquivalentWhenNormalized(StringLiteralExpression("you"));

        public static bool ReturnsStringConcatenation(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.ReturnsStringConcatenationWithDefaultValue() ||
            twoFerSolution.ReturnsStringConcatenationWithNullCoalescingOperator() ||
            twoFerSolution.ReturnsStringConcatenationWithTernaryOperator();

        private static bool ReturnsStringConcatenationWithDefaultValue(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringConcatenationExpression(
                    TwoFerParameterIdentifierName(twoFerSolution)));

        private static bool ReturnsStringConcatenationWithNullCoalescingOperator(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerCoalesceExpression(
                            TwoFerParameterIdentifierName(twoFerSolution)))));

        private static bool ReturnsStringConcatenationWithTernaryOperator(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.ReturnsStringConcatenationWithNullCheck() ||
            twoFerSolution.ReturnsStringConcatenationWithIsNullOrEmptyCheck() ||
            twoFerSolution.ReturnsStringConcatenationWithIsNullOrWhiteSpaceCheck();

        private static bool ReturnsStringConcatenationWithIsNullOrWhiteSpaceCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerConditionalExpression(
                            TwoFerIsNullOrWhiteSpaceInvocationExpression(twoFerSolution),
                            TwoFerParameterIdentifierName(twoFerSolution)))));

        private static bool ReturnsStringConcatenationWithIsNullOrEmptyCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerConditionalExpression(
                            TwoFerIsNullOrEmptyInvocationExpression(twoFerSolution),
                            TwoFerParameterIdentifierName(twoFerSolution)))));

        private static bool ReturnsStringConcatenationWithNullCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerConditionalExpressionWithNullCheck(twoFerSolution))));

        public static bool ReturnsStringFormat(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.ReturnsStringFormatWithDefaultValue() ||
            twoFerSolution.ReturnsStringFormatWithNullCoalescingOperator() ||
            twoFerSolution.ReturnsStringFormatWithTernaryOperator();

        private static bool ReturnsStringFormatWithDefaultValue(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerParameterIdentifierName(twoFerSolution)));

        private static bool ReturnsStringFormatWithNullCoalescingOperator(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerCoalesceExpression(
                        TwoFerParameterIdentifierName(twoFerSolution))));

        private static bool ReturnsStringFormatWithTernaryOperator(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.ReturnsStringFormatWithNullCheck() ||
            twoFerSolution.ReturnsStringFormatWithIsNullOrEmptyCheck() ||
            twoFerSolution.ReturnsStringFormatWithIsNullOrWhiteSpaceCheck();

        private static bool ReturnsStringFormatWithIsNullOrWhiteSpaceCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerConditionalExpression(
                        TwoFerIsNullOrWhiteSpaceInvocationExpression(twoFerSolution),
                        TwoFerParameterIdentifierName(twoFerSolution))));

        private static bool ReturnsStringFormatWithIsNullOrEmptyCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerConditionalExpression(
                        TwoFerIsNullOrEmptyInvocationExpression(twoFerSolution),
                        TwoFerParameterIdentifierName(twoFerSolution))));

        private static bool ReturnsStringFormatWithNullCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerConditionalExpressionWithNullCheck(twoFerSolution)));

        public static bool ReturnsStringInterpolation(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.ReturnsStringInterpolationWithDefaultValue() ||
            twoFerSolution.ReturnsStringInterpolationWithNullCheck() ||
            twoFerSolution.ReturnsStringInterpolationWithNullCoalescingOperator() ||
            twoFerSolution.ReturnsStringInterpolationWithIsNullOrEmptyCheck() ||
            twoFerSolution.ReturnsStringInterpolationWithIsNullOrWhiteSpaceCheck();

        public static bool ReturnsStringInterpolationWithDefaultValue(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerInterpolatedStringExpression(
                    TwoFerParameterIdentifierName(twoFerSolution)));

        public static bool ReturnsStringInterpolationWithNullCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerConditionalInterpolatedStringExpression(
                    EqualsExpression(
                        TwoFerParameterIdentifierName(twoFerSolution),
                        NullLiteralExpression()),
                    TwoFerParameterIdentifierName(twoFerSolution)));

        public static bool ReturnsStringInterpolationWithNullCoalescingOperator(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerInterpolatedStringExpression(
                    TwoFerCoalesceExpression(
                        TwoFerParameterIdentifierName(twoFerSolution))));

        public static bool ReturnsStringInterpolationWithIsNullOrEmptyCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerConditionalInterpolatedStringExpression(
                    TwoFerIsNullOrEmptyInvocationExpression(twoFerSolution),
                    TwoFerParameterIdentifierName(twoFerSolution)));

        public static bool ReturnsStringInterpolationWithIsNullOrWhiteSpaceCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerConditionalInterpolatedStringExpression(
                    TwoFerIsNullOrWhiteSpaceInvocationExpression(twoFerSolution),
                    TwoFerParameterIdentifierName(twoFerSolution)));

        public static bool AssignsParameterUsingKnownExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AssignsParameterUsingNullCoalescingOperator() ||
            twoFerSolution.AssignsParameterUsingNullCheck() ||
            twoFerSolution.AssignsParameterUsingIfNullCheck() ||
            twoFerSolution.AssignsParameterUsingIsNullOrEmptyCheck() ||
            twoFerSolution.AssignsParameterUsingIfIsNullOrEmptyCheck() ||
            twoFerSolution.AssignsParameterUsingIsNullOrWhiteSpaceCheck() ||
            twoFerSolution.AssignsParameterUsingIfIsNullOrWhiteSpaceCheck();

        public static bool AssignsParameterUsingNullCoalescingOperator(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.ParameterAssignedUsingStatement(
                TwoFerAssignParameterStatement(
                    TwoFerCoalesceExpression(
                        TwoFerParameterIdentifierName(twoFerSolution)),
                    TwoFerParameterIdentifierName(twoFerSolution)));

        public static bool AssignsParameterUsingNullCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.ParameterAssignedUsingStatement(
                TwoFerAssignParameterStatement(
                    TwoFerParameterIsNullConditionalExpression(twoFerSolution),
                    TwoFerParameterIdentifierName(twoFerSolution)));

        public static bool AssignsParameterUsingIfNullCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.ParameterAssignedUsingStatement(
                TwoFerAssignParameterIfStatement(
                    TwoFerParameterIsNullExpression(twoFerSolution),
                    TwoFerParameterIdentifierName(twoFerSolution)));

        public static bool AssignsParameterUsingIsNullOrEmptyCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.ParameterAssignedUsingStatement(
                TwoFerAssignParameterStatement(
                    TwoFerParameterIsNullOrEmptyConditionalExpression(twoFerSolution),
                    TwoFerParameterIdentifierName(twoFerSolution)));

        public static bool AssignsParameterUsingIfIsNullOrEmptyCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.ParameterAssignedUsingStatement(
                TwoFerAssignParameterIfStatement(
                    TwoFerIsNullOrEmptyInvocationExpression(twoFerSolution),
                    TwoFerParameterIdentifierName(twoFerSolution)));

        public static bool AssignsParameterUsingIsNullOrWhiteSpaceCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.ParameterAssignedUsingStatement(
                TwoFerAssignParameterStatement(
                    TwoFerParameterIsNullOrWhiteSpaceConditionalExpression(twoFerSolution),
                    TwoFerParameterIdentifierName(twoFerSolution)));

        public static bool AssignsParameterUsingIfIsNullOrWhiteSpaceCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.ParameterAssignedUsingStatement(
                TwoFerAssignParameterIfStatement(
                    TwoFerIsNullOrWhiteSpaceInvocationExpression(twoFerSolution),
                    TwoFerParameterIdentifierName(twoFerSolution)));

        private static bool ParameterAssignedUsingStatement(this TwoFerSolution twoFerSolution, SyntaxNode statement) =>
            twoFerSolution.AssignmentStatement().IsEquivalentWhenNormalized(statement);

        private static StatementSyntax AssignmentStatement(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.Body.Statements[0];

        public static bool AssignsVariable(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Variable != null;

        public static bool AssignsVariableUsingKnownInitializer(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AssignsVariableUsingNullCoalescingOperator() ||
            twoFerSolution.AssignsVariableUsingNullCheck() ||
            twoFerSolution.AssignsVariableUsingIsNullOrEmptyCheck() ||
            twoFerSolution.AssignsVariableUsingIsNullOrWhiteSpaceCheck();

        public static bool AssignsVariableUsingNullCoalescingOperator(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AssignsVariableUsingExpression(
                TwoFerCoalesceExpression(
                    TwoFerParameterIdentifierName(twoFerSolution)));

        public static bool AssignsVariableUsingNullCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AssignsVariableUsingExpression(
                TwoFerParameterIsNullConditionalExpression(twoFerSolution));

        public static bool AssignsVariableUsingIsNullOrEmptyCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AssignsVariableUsingExpression(
                TwoFerParameterIsNullOrEmptyConditionalExpression(twoFerSolution));

        public static bool AssignsVariableUsingIsNullOrWhiteSpaceCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AssignsVariableUsingExpression(
                TwoFerParameterIsNullOrWhiteSpaceConditionalExpression(twoFerSolution));

        private static bool AssignsVariableUsingExpression(this TwoFerSolution twoFerSolution, ExpressionSyntax initializer) =>
            twoFerSolution.Variable.Initializer.IsEquivalentWhenNormalized(
                EqualsValueClause(initializer));

        public static bool ReturnsStringInterpolationWithVariable(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerInterpolatedStringExpression(
                    TwoFerVariableIdentifierName(twoFerSolution)));

        public static bool ReturnsStringConcatenationWithVariable(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringConcatenationExpression(
                    TwoFerVariableIdentifierName(twoFerSolution)));

        public static bool ReturnsStringFormatWithVariable(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerVariableIdentifierName(twoFerSolution)));

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