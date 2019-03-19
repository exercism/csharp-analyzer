using static Exercism.Analyzers.CSharp.Analyzers.TwoFerSolutions;
using static Exercism.Analyzers.CSharp.Analyzers.DefaultComments;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFerComments;

namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class TwoFerAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution)
        {
            if (parsedSolution.IsEquivalentTo(DefaultValueWithStringInterpolationInExpressionBody) ||
                parsedSolution.IsEquivalentTo(StringInterpolationWithInlinedNullCoalescingOperatorInExpressionBody) ||
                parsedSolution.IsEquivalentTo(StringInterpolationWithNullCoalescingOperatorAndVariableForName))
                return parsedSolution.ApproveAsOptimal();

            if (parsedSolution.IsEquivalentTo(StringInterpolationWithTernaryOperatorInExpressionBody) ||
                parsedSolution.IsEquivalentTo(StringInterpolationWithTernaryOperatorInBlockBody))
                return parsedSolution.ApproveWithComment(UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck);

            if (parsedSolution.IsEquivalentTo(DefaultValueWithStringInterpolationInBlockBody) ||
                parsedSolution.IsEquivalentTo(StringInterpolationWithInlinedNullCoalescingOperatorInBlockBody))
                return parsedSolution.ApproveWithComment(UseExpressionBodiedMember);

            if (parsedSolution.IsEquivalentTo(DefaultValueWithStringConcatenationInExpressionBody) ||
                parsedSolution.IsEquivalentTo(DefaultValueWithStringConcatenationInBlockBody) ||
                parsedSolution.IsEquivalentTo(StringConcatenationWithInlinedNullCoalescingOperatorInExpressionBody) ||
                parsedSolution.IsEquivalentTo(StringConcatenationWithInlinedNullCoalescingOperatorInBlockBody))
                return parsedSolution.ApproveWithComment(UseStringInterpolationNotStringConcatenation);

            if (parsedSolution.IsEquivalentTo(DefaultValueWithStringFormatInExpressionBody) ||
                parsedSolution.IsEquivalentTo(DefaultValueWithStringFormatInBlockBody) ||
                parsedSolution.IsEquivalentTo(StringFormatWithInlinedNullCoalescingOperatorInExpressionBody) ||
                parsedSolution.IsEquivalentTo(StringFormatWithInlinedNullCoalescingOperatorInBlockBody))
                return parsedSolution.ApproveWithComment(UseStringInterpolationNotStringFormat);

            if (parsedSolution.IsEquivalentTo(StringConcatenationWithIfStatementUsingBlockDelimitersInBlockBody) ||
                parsedSolution.IsEquivalentTo(StringConcatenationWithIfStatementWithoutBlockDelimitersAndNoElseInBlockBody) ||
                parsedSolution.IsEquivalentTo(StringConcatenationWithIfStatementWithoutBlockDelimitersInBlockBody))
                return parsedSolution.DisapproveWithComment(UseSingleFormattedStringNotMultiple);

            return parsedSolution.ReferToMentor();
        }
    }
}