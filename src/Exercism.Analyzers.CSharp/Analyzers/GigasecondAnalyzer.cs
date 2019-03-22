using static Exercism.Analyzers.CSharp.Analyzers.GigasecondSolutions;
using static Exercism.Analyzers.CSharp.Analyzers.GigasecondComments;
using static Exercism.Analyzers.CSharp.Analyzers.SharedComments;

namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class GigasecondAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution)
        {
            if (parsedSolution.IsEquivalentTo(AddSecondsWithScientificNotationInExpressionBody))
                return parsedSolution.ApproveAsOptimal();

            if (parsedSolution.IsEquivalentTo(AddSecondsWithScientificNotationInBlockBody))
                return parsedSolution.ApproveWithComment(UseExpressionBodiedMember);

            if (parsedSolution.IsEquivalentTo(AddSecondsWithMathPowInExpressionBody) ||
                parsedSolution.IsEquivalentTo(AddSecondsWithMathPowInBlockBody))
                return parsedSolution.ApproveWithComment(UseScientificNotationNotMathPow);

            if (parsedSolution.IsEquivalentTo(AddSecondsWithDigitsWithoutSeparatorInExpressionBody) ||
                parsedSolution.IsEquivalentTo(AddSecondsWithDigitsWithoutSeparatorInBlockBody))
                return parsedSolution.ApproveWithComment(UseScientificNotationOrDigitSeparators);

            if (parsedSolution.IsEquivalentTo(AddInExpressionBody) ||
                parsedSolution.IsEquivalentTo(AddInBlockBody) ||
                parsedSolution.IsEquivalentTo(PlusOperatorInExpressionBody) ||
                parsedSolution.IsEquivalentTo(PlusOperatorInBlockBody))
                return parsedSolution.DisapproveWithComment(UseAddSeconds);

            return parsedSolution.ReferToMentor();
        }
    }
}