using static Exercism.Analyzers.CSharp.Analyzers.TwoFerSolutions;
using static Exercism.Analyzers.CSharp.Analyzers.DefaultComments;

namespace Exercism.Analyzers.CSharp.Analyzers
{   
    internal static class TwoFerAnalyzer
    {
        public static SolutionAnalysis Analyze(CompiledSolution compiledSolution)
        {
            if (compiledSolution.IsEquivalentTo(DefaultValueWithStringInterpolationInExpressionBody))
                return compiledSolution.ApproveAsOptimal();

            if (compiledSolution.IsEquivalentTo(DefaultValueWithStringInterpolationInBlockBody))
                return compiledSolution.ApproveWithComment(UseExpressionBodiedMember);

            if (compiledSolution.IsEquivalentTo(DefaultValueWithStringConcatenationInExpressionBody))
                return compiledSolution.ApproveWithComment(UseStringInterpolationNotStringConcatenation);

            if (compiledSolution.IsEquivalentTo(DefaultValueWithStringConcatenationInBlockBody))
                return compiledSolution.ApproveWithComment(UseStringInterpolationNotStringConcatenation);
            
            if (compiledSolution.IsEquivalentTo(DefaultValueWithStringFormatInExpressionBody))
                return compiledSolution.ApproveWithComment(UseStringInterpolationNotStringFormat);

            if (compiledSolution.IsEquivalentTo(DefaultValueWithStringFormatInBlockBody))
                return compiledSolution.ApproveWithComment(UseStringInterpolationNotStringFormat);

            if (compiledSolution.IsEquivalentTo(StringInterpolationWithInlinedNullCoalescingOperatorInExpressionBody))
                return compiledSolution.ApproveAsOptimal();

            if (compiledSolution.IsEquivalentTo(StringInterpolationWithInlinedNullCoalescingOperatorInBlockBody))
                return compiledSolution.ApproveWithComment(UseExpressionBodiedMember);
            
            if (compiledSolution.IsEquivalentTo(StringConcatenationWithInlinedNullCoalescingOperatorInExpressionBody))
                return compiledSolution.ApproveWithComment(UseStringInterpolationNotStringConcatenation);

            if (compiledSolution.IsEquivalentTo(StringConcatenationWithInlinedNullCoalescingOperatorInBlockBody))
                return compiledSolution.ApproveWithComment(UseStringInterpolationNotStringConcatenation);
            
            if (compiledSolution.IsEquivalentTo(StringFormatWithInlinedNullCoalescingOperatorInExpressionBody))
                return compiledSolution.ApproveWithComment(UseStringInterpolationNotStringFormat);

            if (compiledSolution.IsEquivalentTo(StringFormatWithInlinedNullCoalescingOperatorInBlockBody))
                return compiledSolution.ApproveWithComment(UseStringInterpolationNotStringFormat);

            if (compiledSolution.IsEquivalentTo(StringInterpolationWithNullCoalescingOperatorAndVariableForName))
                return compiledSolution.ApproveAsOptimal();

            return compiledSolution.ReferToMentor();
        }
    }
}