using Exercism.Analyzers.CSharp.Analyzers.Shared;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerSyntax;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal static class TwoFerVariableAssignmentAnalyzer
    {
        public static SolutionAnalysis AnalyzeVariableAssignment(this TwoFerSolution twoFerSolution)
        {
            if (twoFerSolution.Variable == null)
                return null;

            if (!twoFerSolution.AssignsVariableUsingKnownInitializer())
                return twoFerSolution.ReferToMentor();

            if (twoFerSolution.UsesStringFormatWithVariable())
                return twoFerSolution.ApproveWithComment(SharedComments.UseStringInterpolationNotStringFormat);

            if (twoFerSolution.UsesStringConcatenationWithVariable())
                return twoFerSolution.ApproveWithComment(SharedComments.UseStringInterpolationNotStringConcatenation);

            if (!twoFerSolution.UsesStringInterpolationWithVariable())
                return null;

            if (twoFerSolution.VariableAssignedUsingNullCoalescingOperator())
                return twoFerSolution.ApproveAsOptimal();

            if (twoFerSolution.VariableAssignedUsingNullCheck())
                return twoFerSolution.ApproveWithComment(SharedComments.UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck);

            if (twoFerSolution.VariableAssignedUsingIsNullOrEmpty())
                return twoFerSolution.ApproveWithComment(TwoFerComments.UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrEmptyCheck);

            if (twoFerSolution.VariableAssignedUsingIsNullOrWhiteSpace())
                return twoFerSolution.ApproveWithComment(TwoFerComments.UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrWhiteSpaceCheck);

            return null;
        }

        private static bool AssignsVariableUsingKnownInitializer(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.VariableAssignedUsingNullCoalescingOperator() ||
            twoFerSolution.VariableAssignedUsingNullCheck() ||
            twoFerSolution.VariableAssignedUsingIsNullOrEmpty() ||
            twoFerSolution.VariableAssignedUsingIsNullOrWhiteSpace();

        private static bool VariableAssignedUsingNullCoalescingOperator(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Variable.Initializer.IsEquivalentWhenNormalized(
                EqualsValueClause(
                    TwoFerCoalesceExpression(
                        TwoFerParameterIdentifierName(twoFerSolution))));

        private static bool VariableAssignedUsingNullCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Variable.Initializer.IsEquivalentWhenNormalized(
                EqualsValueClause(
                    TwoFerConditionalExpression(
                        TwoFerParameterIsNullExpression(twoFerSolution), 
                        TwoFerParameterIdentifierName(twoFerSolution))));

        private static bool VariableAssignedUsingIsNullOrEmpty(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Variable.Initializer.IsEquivalentWhenNormalized(
                EqualsValueClause(
                    TwoFerConditionalExpression(
                        TwoFerIsNullOrEmptyInvocationExpression(twoFerSolution),
                        TwoFerParameterIdentifierName(twoFerSolution))));

        private static bool VariableAssignedUsingIsNullOrWhiteSpace(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Variable.Initializer.IsEquivalentWhenNormalized(
                EqualsValueClause(
                    TwoFerConditionalExpression(
                        TwoFerIsNullOrWhiteSpaceInvocationExpression(twoFerSolution),
                        TwoFerParameterIdentifierName(twoFerSolution))));

        private static bool UsesStringInterpolationWithVariable(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerInterpolatedStringExpression(
                    IdentifierName(twoFerSolution.Variable.Identifier)));

        private static bool UsesStringConcatenationWithVariable(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringConcatenationExpression(
                    IdentifierName(twoFerSolution.Variable.Identifier)));

        private static bool UsesStringFormatWithVariable(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Returns(
                TwoFerStringFormatInvocationExpression(
                    IdentifierName(twoFerSolution.Variable.Identifier)));

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