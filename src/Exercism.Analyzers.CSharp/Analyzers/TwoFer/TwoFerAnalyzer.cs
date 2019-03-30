using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerComments;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerSyntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal static class TwoFerAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution) =>
            Analyze(new TwoFerSolution(parsedSolution));

        private static SolutionAnalysis Analyze(TwoFerSolution twoFerSolution) =>
            twoFerSolution.AnalyzeError() ??
            twoFerSolution.AnalyzeSingleLine() ??
            twoFerSolution.AnalyzeMultiLine() ??
            twoFerSolution.ReferToMentor();

        private static SolutionAnalysis AnalyzeError(this TwoFerSolution twoFerSolution)
        {
            if (twoFerSolution.UsesOverloads())
                return twoFerSolution.DisapproveWithComment(UseSingleFormattedStringNotMultiple);

            if (twoFerSolution.MissingNameMethod() ||
                twoFerSolution.InvalidNameMethod())
                return twoFerSolution.DisapproveWithComment(FixCompileErrors);

            if (twoFerSolution.UsesDuplicateString())
                return twoFerSolution.DisapproveWithComment(UseSingleFormattedStringNotMultiple);

            if (twoFerSolution.NoDefaultValue())
                return twoFerSolution.DisapproveWithComment(UseDefaultValue);

            if (twoFerSolution.UsesInvalidDefaultValue())
                return twoFerSolution.DisapproveWithComment(InvalidDefaultValue);

            if (twoFerSolution.UsesStringReplace())
                return twoFerSolution.DisapproveWithComment(UseStringInterpolationNotStringReplace);

            if (twoFerSolution.UsesStringJoin())
                return twoFerSolution.DisapproveWithComment(UseStringInterpolationNotStringJoin);

            if (twoFerSolution.UsesStringConcat())
                return twoFerSolution.DisapproveWithComment(UseStringInterpolationNotStringConcat);

            return null;
        }

        private static SolutionAnalysis AnalyzeSingleLine(this TwoFerSolution twoFerSolution)
        {
            if (!twoFerSolution.NameMethod.SingleExpression())
                return null;

            if (twoFerSolution.NameMethod.IsExpressionBody() &&
                (twoFerSolution.ReturnedExpression.IsDefaultInterpolatedStringExpression(twoFerSolution.InputParameter) ||
                 twoFerSolution.ReturnedExpression.IsNullCoalescingInterpolatedStringExpression(twoFerSolution.InputParameter)))
                return twoFerSolution.ApproveAsOptimal();

            if (twoFerSolution.ReturnedExpression.IsDefaultInterpolatedStringExpression(twoFerSolution.InputParameter) ||
                twoFerSolution.ReturnedExpression.IsNullCoalescingInterpolatedStringExpression(twoFerSolution.InputParameter))
                return twoFerSolution.ApproveWithComment(UseExpressionBodiedMember);

            if (twoFerSolution.ReturnedExpression.IsIsNullOrEmptyInterpolatedStringExpression(twoFerSolution.InputParameter))
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrEmptyCheck);

            if (twoFerSolution.ReturnedExpression.IsIsNullOrWhiteSpaceInterpolatedStringExpression(twoFerSolution.InputParameter))
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrWhiteSpaceCheck);

            if (twoFerSolution.ReturnedExpression.IsTernaryOperatorInterpolatedStringExpression(twoFerSolution.InputParameter))
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck);

            if (twoFerSolution.UsesStringConcatenation())
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringConcatenation);

            if (twoFerSolution.UsesStringFormat())
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringFormat);

            return null;
        }

        private static SolutionAnalysis AnalyzeMultiLine(this TwoFerSolution twoFerSolution) =>
            AnalyzeParameterAssignment(twoFerSolution) ??
            AnalyzeVariableAssignment(twoFerSolution);

        private static SolutionAnalysis AnalyzeParameterAssignment(TwoFerSolution twoFerSolution)
        {
            if (!twoFerSolution.AssignsToParameter())
                return null;

            if (!twoFerSolution.AssignsToParameterUsingKnownCheck())
                return twoFerSolution.ReferToMentor();

            if (twoFerSolution.ReturnedExpression.IsEquivalentToNormalized(
                TwoFerStringFormatInvocationExpression(
                    IdentifierName(twoFerSolution.InputParameter.Identifier))))
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringFormat);

            if (twoFerSolution.ReturnedExpression.IsEquivalentToNormalized(
                TwoFerStringConcatenationExpression(
                    IdentifierName(twoFerSolution.InputParameter.Identifier))))
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringConcatenation);

            if (!twoFerSolution.ReturnedExpression.IsEquivalentToNormalized(
                TwoFerInterpolatedStringExpression(
                    IdentifierName(twoFerSolution.InputParameter.Identifier))))
                return null;

            if (twoFerSolution.AssignsToParameterUsingNullCoalescingCheck())
                return twoFerSolution.ApproveWithComment(InlineVariable);

            if (twoFerSolution.AssignsToParameterUsingNullCheck())
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotNullCheck);

            if (twoFerSolution.AssignsToParameterUsingIsNullOrEmptyCheck())
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotIsNullOrEmptyCheck);

            if (twoFerSolution.AssignsToParameterUsingIsNullOrWhiteSpaceCheck())
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotIsNullOrWhiteSpaceCheck);

            return null;
        }

        private static SolutionAnalysis AnalyzeVariableAssignment(TwoFerSolution twoFerSolution)
        {
            var variableDeclarator = twoFerSolution.NameMethod.AssignedVariable();
            if (variableDeclarator == null)
                return null;

            if (!twoFerSolution.AssignsVariableUsingKnownInitializer())
                return twoFerSolution.ReferToMentor();

            if (twoFerSolution.ReturnedExpression.IsEquivalentToNormalized(
                TwoFerStringFormatInvocationExpression(
                    IdentifierName(variableDeclarator.Identifier))))
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringFormat);

            if (twoFerSolution.ReturnedExpression.IsEquivalentToNormalized(
                TwoFerStringConcatenationExpression(
                    IdentifierName(variableDeclarator.Identifier))))
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringConcatenation);

            if (!twoFerSolution.ReturnedExpression.IsEquivalentToNormalized(
                TwoFerInterpolatedStringExpression(
                    IdentifierName(variableDeclarator.Identifier))))
                return null;

            if (twoFerSolution.VariableAssignedUsingNullCoalescingOperator())
                return twoFerSolution.ApproveAsOptimal();

            if (twoFerSolution.VariableAssignedUsingNullCheck())
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck);

            if (twoFerSolution.VariableAssignedUsingIsNullOrEmpty())
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrEmptyCheck);

            if (twoFerSolution.VariableAssignedUsingIsNullOrWhiteSpace())
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrWhiteSpaceCheck);

            return null;
        }
    }
}