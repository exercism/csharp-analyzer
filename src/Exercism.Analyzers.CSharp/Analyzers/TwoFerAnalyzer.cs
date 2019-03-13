using static Exercism.Analyzers.CSharp.Analyzers.TwoFerSolutions;

namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class TwoFerAnalyzer
    {
        public static SolutionAnalysis Analyze(CompiledSolution compiledSolution)
        {
            if (compiledSolution.IsEquivalentTo(DefaultValueWithStringInterpolationInExpressionBody))
                return compiledSolution.ApproveAsOptimal();

            if (compiledSolution.IsEquivalentTo(DefaultValueWithStringInterpolationInBlockBody))
                return compiledSolution.ApproveWithComment("You could write the method an an expression-bodied member");

            if (compiledSolution.IsEquivalentTo(DefaultValueWithStringConcatenationInExpressionBody))
                return compiledSolution.ApproveWithComment("You can use string interpolation");

            if (compiledSolution.IsEquivalentTo(DefaultValueWithStringConcatenationInBlockBody))
                return compiledSolution.ApproveWithComment("You can use string interpolation");
            
            if (compiledSolution.IsEquivalentTo(DefaultValueWithStringFormatInExpressionBody))
                return compiledSolution.ApproveWithComment("You can use string interpolation");

            if (compiledSolution.IsEquivalentTo(DefaultValueWithStringFormatInBlockBody))
                return compiledSolution.ApproveWithComment("You can use string interpolation");

            if (compiledSolution.IsEquivalentTo(StringInterpolationWithInlinedNullCoalescingOperatorInExpressionBody))
                return compiledSolution.ApproveAsOptimal();

            if (compiledSolution.IsEquivalentTo(StringInterpolationWithInlinedNullCoalescingOperatorInBlockBody))
                return compiledSolution.ApproveWithComment("You could write the method an an expression-bodied member");
            
            if (compiledSolution.IsEquivalentTo(StringConcatenationWithInlinedNullCoalescingOperatorInExpressionBody))
                return compiledSolution.ApproveWithComment("You can use string interpolation");

            if (compiledSolution.IsEquivalentTo(StringConcatenationWithInlinedNullCoalescingOperatorInBlockBody))
                return compiledSolution.ApproveWithComment("You can use string interpolation");
            
            if (compiledSolution.IsEquivalentTo(StringFormatWithInlinedNullCoalescingOperatorInExpressionBody))
                return compiledSolution.ApproveWithComment("You can use string interpolation");

            if (compiledSolution.IsEquivalentTo(StringFormatWithInlinedNullCoalescingOperatorInBlockBody))
                return compiledSolution.ApproveWithComment("You can use string interpolation");

            if (compiledSolution.IsEquivalentTo(StringInterpolationWithNullCoalescingOperatorAndVariableForName))
                return compiledSolution.ApproveAsOptimal();

            return compiledSolution.ReferToMentor();
        }
    }
}