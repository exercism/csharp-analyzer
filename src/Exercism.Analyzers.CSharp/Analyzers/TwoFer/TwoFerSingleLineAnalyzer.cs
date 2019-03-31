using Exercism.Analyzers.CSharp.Analyzers.Shared;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal static class TwoFerSingleLineAnalyzer
    {
        public static SolutionAnalysis AnalyzeSingleLine(this TwoFerSolution twoFerSolution)
        {
            if (!twoFerSolution.NameMethod.SingleExpression())
                return null;

            if (twoFerSolution.NameMethod.IsExpressionBody() &&
                (twoFerSolution.UsesDefaultInterpolatedStringExpression() ||
                 twoFerSolution.UsesNullCoalescingInInterpolatedStringExpression()))
                return twoFerSolution.ApproveAsOptimal();

            if (twoFerSolution.UsesDefaultInterpolatedStringExpression() ||
                twoFerSolution.UsesNullCoalescingInInterpolatedStringExpression())
                return twoFerSolution.ApproveWithComment(SharedComments.UseExpressionBodiedMember);

            if (twoFerSolution.UsesIsNullOrEmptyInInterpolatedStringExpression())
                return twoFerSolution.ApproveWithComment(TwoFerComments.UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrEmptyCheck);

            if (twoFerSolution.UsesIsNullOrWhiteSpaceInInterpolatedStringExpression())
                return twoFerSolution.ApproveWithComment(TwoFerComments.UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrWhiteSpaceCheck);

            if (twoFerSolution.UsesTernaryOperatorInInterpolatedStringExpression())
                return twoFerSolution.ApproveWithComment(SharedComments.UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck);

            if (twoFerSolution.UsesStringConcatenation())
                return twoFerSolution.ApproveWithComment(SharedComments.UseStringInterpolationNotStringConcatenation);

            if (twoFerSolution.UsesStringFormat())
                return twoFerSolution.ApproveWithComment(SharedComments.UseStringInterpolationNotStringFormat);

            return null;
        }
    }
}