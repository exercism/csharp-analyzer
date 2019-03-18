using static Exercism.Analyzers.CSharp.Analyzers.TwoFerSolutions;
using static Exercism.Analyzers.CSharp.Analyzers.DefaultComments;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFerComments;

namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class TwoFerAnalyzer
    {
        public static SolutionAnalysis Analyze(CompiledSolution compiledSolution)
        {
            if (compiledSolution.IsEquivalentTo(DefaultValueWithStringInterpolationInExpressionBody) ||
                compiledSolution.IsEquivalentTo(StringInterpolationWithInlinedNullCoalescingOperatorInExpressionBody) ||
                compiledSolution.IsEquivalentTo(StringInterpolationWithNullCoalescingOperatorAndVariableForName))
                return compiledSolution.ApproveAsOptimal();

            if (compiledSolution.IsEquivalentTo(DefaultValueWithStringInterpolationInBlockBody) ||
                compiledSolution.IsEquivalentTo(StringInterpolationWithInlinedNullCoalescingOperatorInBlockBody))
                return compiledSolution.ApproveWithComment(UseExpressionBodiedMember);

            if (compiledSolution.IsEquivalentTo(DefaultValueWithStringConcatenationInExpressionBody) ||
                compiledSolution.IsEquivalentTo(DefaultValueWithStringConcatenationInBlockBody) ||
                compiledSolution.IsEquivalentTo(StringConcatenationWithInlinedNullCoalescingOperatorInExpressionBody) ||
                compiledSolution.IsEquivalentTo(StringConcatenationWithInlinedNullCoalescingOperatorInBlockBody))
                return compiledSolution.ApproveWithComment(UseStringInterpolationNotStringConcatenation);

            if (compiledSolution.IsEquivalentTo(DefaultValueWithStringFormatInExpressionBody) ||
                compiledSolution.IsEquivalentTo(DefaultValueWithStringFormatInBlockBody) ||
                compiledSolution.IsEquivalentTo(StringFormatWithInlinedNullCoalescingOperatorInExpressionBody) ||
                compiledSolution.IsEquivalentTo(StringFormatWithInlinedNullCoalescingOperatorInBlockBody))
                return compiledSolution.ApproveWithComment(UseStringInterpolationNotStringFormat);

            if (compiledSolution.IsEquivalentTo(StringConcatenationWithIfStatementUsingBlockDelimitersInBlockBody) ||
                compiledSolution.IsEquivalentTo(StringConcatenationWithIfStatementWithoutBlockDelimitersAndNoElseInBlockBody) ||
                compiledSolution.IsEquivalentTo(StringConcatenationWithIfStatementWithoutBlockDelimitersInBlockBody))
                return compiledSolution.DisapproveWithComment(UseSingleFormattedStringNotMultiple);

            return compiledSolution.ReferToMentor();
        }
    }
}