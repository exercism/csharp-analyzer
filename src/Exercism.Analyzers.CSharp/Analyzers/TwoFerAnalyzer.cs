using static Exercism.Analyzers.CSharp.Analyzers.TwoFerSolutions;

namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class TwoFerAnalyzer
    {
        public static SolutionAnalysis Analyze(SolutionImplementation solution)
        {
            if (solution.IsEquivalentTo(DefaultValueWithStringInterpolationInExpressionBody))
                return solution.ApproveAsOptimal();

            if (solution.IsEquivalentTo(DefaultValueWithStringInterpolationInBlockBody))
                return solution.ApproveWithComment("You could write the method an an expression-bodied member");

            if (solution.IsEquivalentTo(DefaultValueWithStringConcatenationInExpressionBody))
                return solution.ApproveWithComment("You can use string interpolation");

            if (solution.IsEquivalentTo(DefaultValueWithStringConcatenationInBlockBody))
                return solution.ApproveWithComment("You can use string interpolation");
            
            if (solution.IsEquivalentTo(DefaultValueWithStringFormatInExpressionBody))
                return solution.ApproveWithComment("You can use string interpolation");

            if (solution.IsEquivalentTo(DefaultValueWithStringFormatInBlockBody))
                return solution.ApproveWithComment("You can use string interpolation");

            if (solution.IsEquivalentTo(StringInterpolationWithInlinedNullCoalescingOperatorInExpressionBody))
                return solution.ApproveAsOptimal();

            if (solution.IsEquivalentTo(StringInterpolationWithInlinedNullCoalescingOperatorInBlockBody))
                return solution.ApproveWithComment("You could write the method an an expression-bodied member");
            
            if (solution.IsEquivalentTo(StringConcatenationWithInlinedNullCoalescingOperatorInExpressionBody))
                return solution.ApproveWithComment("You can use string interpolation");

            if (solution.IsEquivalentTo(StringConcatenationWithInlinedNullCoalescingOperatorInBlockBody))
                return solution.ApproveWithComment("You can use string interpolation");
            
            if (solution.IsEquivalentTo(StringFormatWithInlinedNullCoalescingOperatorInExpressionBody))
                return solution.ApproveWithComment("You can use string interpolation");

            if (solution.IsEquivalentTo(StringFormatWithInlinedNullCoalescingOperatorInBlockBody))
                return solution.ApproveWithComment("You can use string interpolation");

            if (solution.IsEquivalentTo(StringInterpolationWithNullCoalescingOperatorAndVariableForName))
                return solution.ApproveAsOptimal();

            return solution.ReferToMentor();
        }
    }
}