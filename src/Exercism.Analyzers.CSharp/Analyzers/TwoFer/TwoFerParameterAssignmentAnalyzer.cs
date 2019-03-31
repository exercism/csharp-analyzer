using Exercism.Analyzers.CSharp.Analyzers.Shared;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerSyntax;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal static class TwoFerParameterAssignmentAnalyzer
    {
        public static SolutionAnalysis AnalyzeParameterAssignment(this TwoFerSolution twoFerSolution)
        {
            if (!twoFerSolution.AssignsToParameter())
                return null;

            if (!twoFerSolution.AssignsToParameterUsingKnownCheck())
                return twoFerSolution.ReferToMentor();

            if (twoFerSolution.Returns(
                TwoFerStringFormatInvocationExpression(
                    IdentifierName(twoFerSolution.InputParameter.Identifier))))
                return twoFerSolution.ApproveWithComment(SharedComments.UseStringInterpolationNotStringFormat);

            if (twoFerSolution.Returns(
                TwoFerStringConcatenationExpression(
                    IdentifierName(twoFerSolution.InputParameter.Identifier))))
                return twoFerSolution.ApproveWithComment(SharedComments.UseStringInterpolationNotStringConcatenation);

            if (!twoFerSolution.Returns(
                TwoFerInterpolatedStringExpression(
                    IdentifierName(twoFerSolution.InputParameter.Identifier))))
                return null;

            if (twoFerSolution.AssignsToParameterUsingNullCoalescingCheck())
                return twoFerSolution.ApproveWithComment(SharedComments.InlineVariable);

            if (twoFerSolution.AssignsToParameterUsingNullCheck())
                return twoFerSolution.ApproveWithComment(SharedComments.UseNullCoalescingOperatorNotNullCheck);

            if (twoFerSolution.AssignsToParameterUsingIsNullOrEmptyCheck())
                return twoFerSolution.ApproveWithComment(TwoFerComments.UseNullCoalescingOperatorNotIsNullOrEmptyCheck);

            if (twoFerSolution.AssignsToParameterUsingIsNullOrWhiteSpaceCheck())
                return twoFerSolution.ApproveWithComment(TwoFerComments.UseNullCoalescingOperatorNotIsNullOrWhiteSpaceCheck);

            return null;
        }

        private static bool AssignsToParameterUsingKnownCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AssignsToParameterUsingNullCoalescingCheck() ||
            twoFerSolution.AssignsToParameterUsingNullCheck() ||
            twoFerSolution.AssignsToParameterUsingIsNullOrEmptyCheck() ||
            twoFerSolution.AssignsToParameterUsingIsNullOrWhiteSpaceCheck();

        private static bool AssignsToParameterUsingNullCoalescingCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AssignsToParameterUsingExpression(
                ExpressionStatement(
                    SimpleAssignmentExpression(
                        IdentifierName(twoFerSolution.InputParameter.Identifier), 
                        TwoFerCoalesceExpression(TwoFerParameterIdentifierName(twoFerSolution)))));
        private static bool AssignsToParameterUsingNullCheck(this TwoFerSolution twoFerSolution) =>
        twoFerSolution.AssignsToParameterUsingExpression(
            IfStatement(
                TwoFerParameterIsNullExpression(twoFerSolution),
                TwoFerAssignParameterToYou(twoFerSolution)));

        private static bool AssignsToParameterUsingIsNullOrEmptyCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AssignsToParameterUsingExpression(
                IfStatement(
                    TwoFerIsNullOrEmptyInvocationExpression(twoFerSolution),
                    TwoFerAssignParameterToYou(twoFerSolution)));

        private static bool AssignsToParameterUsingIsNullOrWhiteSpaceCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AssignsToParameterUsingExpression(
                IfStatement(
                    TwoFerIsNullOrWhiteSpaceInvocationExpression(twoFerSolution),
                    TwoFerAssignParameterToYou(twoFerSolution)));

        private static bool AssignsToParameterUsingExpression(this TwoFerSolution twoFerSolution, SyntaxNode expressionStatement) =>
            twoFerSolution.AssignmentStatement().IsEquivalentWhenNormalized(expressionStatement);

        private static StatementSyntax AssignmentStatement(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.Body.Statements[0];
    }
}