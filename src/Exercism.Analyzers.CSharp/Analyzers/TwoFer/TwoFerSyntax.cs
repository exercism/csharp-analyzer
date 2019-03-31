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
                .Count(literalExpression => literalExpression.Token.ValueText.Contains("one for me"));

            var interpolatedStringTextCount = twoFerSolution.NameMethod
                .DescendantNodes<InterpolatedStringTextSyntax>()
                .Count(interpolatedStringText => interpolatedStringText.TextToken.ValueText.Contains("one for me"));

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

        public static bool UsesStringConcatenation(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.ReturnsDefaultStringConcatenationExpression() ||
            twoFerSolution.ReturnsNullCoalescingStringConcatenationExpression() ||
            twoFerSolution.ReturnsTernaryOperatorStringConcatenationExpression();

        private static bool ReturnsDefaultStringConcatenationExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringConcatenationExpression(
                    TwoFerParameterIdentifierName(twoFerSolution)));

        private static bool ReturnsNullCoalescingStringConcatenationExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerCoalesceExpression(
                            TwoFerParameterIdentifierName(twoFerSolution)))));

        private static bool ReturnsTernaryOperatorStringConcatenationExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.ReturnsNullCheckTernaryOperatorStringConcatenationExpression() ||
            twoFerSolution.ReturnsIsNullOrEmptyTernaryOperatorStringConcatenationExpression() ||
            twoFerSolution.ReturnsIsNullOrWhiteSpaceTernaryOperatorStringConcatenationExpression();

        private static bool ReturnsIsNullOrWhiteSpaceTernaryOperatorStringConcatenationExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerConditionalExpression(
                            TwoFerIsNullOrWhiteSpaceInvocationExpression(twoFerSolution),
                            TwoFerParameterIdentifierName(twoFerSolution)))));

        private static bool ReturnsIsNullOrEmptyTernaryOperatorStringConcatenationExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerConditionalExpression(
                            TwoFerIsNullOrEmptyInvocationExpression(twoFerSolution),
                            TwoFerParameterIdentifierName(twoFerSolution)))));

        private static bool ReturnsNullCheckTernaryOperatorStringConcatenationExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerTernaryOperatorConditionalExpression(twoFerSolution.InputParameter))));

        public static bool UsesStringFormat(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.ReturnsDefaultStringFormatExpression() ||
            twoFerSolution.ReturnsNullCoalescingStringFormatExpression() ||
            twoFerSolution.ReturnsTernaryOperatorStringFormatExpression();

        private static bool ReturnsDefaultStringFormatExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerParameterIdentifierName(twoFerSolution)));

        private static bool ReturnsNullCoalescingStringFormatExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerCoalesceExpression(
                        TwoFerParameterIdentifierName(twoFerSolution))));

        private static bool ReturnsTernaryOperatorStringFormatExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.ReturnsNullCheckTernaryOperatorStringFormatExpression() ||
            twoFerSolution.ReturnsIsNullOrEmptyTernaryOperatorStringFormatExpression() ||
            twoFerSolution.ReturnsIsNullOrWhiteSpaceTernaryOperatorStringFormatExpression();

        private static bool ReturnsIsNullOrWhiteSpaceTernaryOperatorStringFormatExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerConditionalExpression(
                        TwoFerIsNullOrWhiteSpaceInvocationExpression(twoFerSolution),
                        TwoFerParameterIdentifierName(twoFerSolution))));

        private static bool ReturnsIsNullOrEmptyTernaryOperatorStringFormatExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerConditionalExpression(
                        TwoFerIsNullOrEmptyInvocationExpression(twoFerSolution),
                        TwoFerParameterIdentifierName(twoFerSolution))));

        private static bool ReturnsNullCheckTernaryOperatorStringFormatExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerTernaryOperatorConditionalExpression(twoFerSolution.InputParameter)));

        public static bool UsesStringInterpolation(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.ReturnsDefaultInterpolatedStringExpression() ||
            twoFerSolution.ReturnsTernaryOperatorInInterpolatedStringExpression() ||
            twoFerSolution.ReturnsNullCoalescingInInterpolatedStringExpression() ||
            twoFerSolution.ReturnsIsNullOrEmptyInInterpolatedStringExpression() ||
            twoFerSolution.ReturnsIsNullOrWhiteSpaceInInterpolatedStringExpression();
        
        public static bool ReturnsDefaultInterpolatedStringExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerInterpolatedStringExpression(
                    TwoFerParameterIdentifierName(twoFerSolution)));

        public static bool ReturnsTernaryOperatorInInterpolatedStringExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerConditionalInterpolatedStringExpression(
                    EqualsExpression(
                        TwoFerParameterIdentifierName(twoFerSolution),
                        NullLiteralExpression()),
                    TwoFerParameterIdentifierName(twoFerSolution)));

        public static bool ReturnsNullCoalescingInInterpolatedStringExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerInterpolatedStringExpression(
                    TwoFerCoalesceExpression(
                        TwoFerParameterIdentifierName(twoFerSolution))));

        public static bool ReturnsIsNullOrEmptyInInterpolatedStringExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerConditionalInterpolatedStringExpression(TwoFerIsNullOrEmptyInvocationExpression(twoFerSolution), TwoFerParameterIdentifierName(twoFerSolution)));

        public static bool ReturnsIsNullOrWhiteSpaceInInterpolatedStringExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerConditionalInterpolatedStringExpression(TwoFerIsNullOrWhiteSpaceInvocationExpression(twoFerSolution), TwoFerParameterIdentifierName(twoFerSolution)));

        public static bool ParameterAssignedUsingKnownExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AssignsParameterUsingNullCoalescingExpression() ||
            twoFerSolution.AssignsParameterUsingNullCheck() ||
            twoFerSolution.AssignsParameterUsingIsNullOrEmptyCheck() ||
            twoFerSolution.AssignsParameterUsingIsNullOrWhiteSpaceCheck();

        public static bool AssignsParameterUsingNullCoalescingExpression(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.ParameterAssignedUsingExpression(
                ExpressionStatement(
                    SimpleAssignmentExpression(
                        IdentifierName(twoFerSolution.InputParameter.Identifier), 
                        TwoFerCoalesceExpression(TwoFerParameterIdentifierName(twoFerSolution)))));

        public static bool AssignsParameterUsingNullCheck(this TwoFerSolution twoFerSolution) =>
        twoFerSolution.ParameterAssignedUsingExpression(
            IfStatement(
                TwoFerParameterIsNullExpression(twoFerSolution),
                TwoFerAssignParameterToYou(twoFerSolution)));

        public static bool AssignsParameterUsingIsNullOrEmptyCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.ParameterAssignedUsingExpression(
                IfStatement(
                    TwoFerIsNullOrEmptyInvocationExpression(twoFerSolution),
                    TwoFerAssignParameterToYou(twoFerSolution)));

        public static bool AssignsParameterUsingIsNullOrWhiteSpaceCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.ParameterAssignedUsingExpression(
                IfStatement(
                    TwoFerIsNullOrWhiteSpaceInvocationExpression(twoFerSolution),
                    TwoFerAssignParameterToYou(twoFerSolution)));

        private static bool ParameterAssignedUsingExpression(this TwoFerSolution twoFerSolution, SyntaxNode expressionStatement) =>
            twoFerSolution.AssignmentStatement().IsEquivalentWhenNormalized(expressionStatement);

        private static StatementSyntax AssignmentStatement(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.Body.Statements[0];

        public static bool AssignsVariable(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Variable != null;

        public static bool AssignsVariableUsingKnownInitializer(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AssignsVariableUsingNullCoalescingOperator() ||
            twoFerSolution.AssignsVariableUsingNullCheck() ||
            twoFerSolution.AssignsVariableUsingIsNullOrEmpty() ||
            twoFerSolution.AssignsVariableUsingIsNullOrWhiteSpace();

        public static bool AssignsVariableUsingNullCoalescingOperator(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AssignsVariableUsingExpression(
                TwoFerCoalesceExpression(
                    TwoFerParameterIdentifierName(twoFerSolution)));

        public static bool AssignsVariableUsingNullCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AssignsVariableUsingExpression(
                TwoFerConditionalExpression(
                    TwoFerParameterIsNullExpression(twoFerSolution), 
                    TwoFerParameterIdentifierName(twoFerSolution)));

        public static bool AssignsVariableUsingIsNullOrEmpty(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AssignsVariableUsingExpression(
                TwoFerConditionalExpression(
                    TwoFerIsNullOrEmptyInvocationExpression(twoFerSolution),
                    TwoFerParameterIdentifierName(twoFerSolution)));

        public static bool AssignsVariableUsingIsNullOrWhiteSpace(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AssignsVariableUsingExpression(
                TwoFerConditionalExpression(
                    TwoFerIsNullOrWhiteSpaceInvocationExpression(twoFerSolution),
                    TwoFerParameterIdentifierName(twoFerSolution)));

        private static bool AssignsVariableUsingExpression(this TwoFerSolution twoFerSolution, ExpressionSyntax initializer) =>
            twoFerSolution.Variable.Initializer.IsEquivalentWhenNormalized(EqualsValueClause(initializer));

        public static bool ReturnsStringInterpolationUsingVariable(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerInterpolatedStringExpression(
                    TwoFerVariableIdentifierName(twoFerSolution)));

        public static bool ReturnsStringConcatenationUsingVariable(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringConcatenationExpression(
                    TwoFerVariableIdentifierName(twoFerSolution)));

        public static bool ReturnsStringFormatUsingVariable(this TwoFerSolution twoFerSolution) =>
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