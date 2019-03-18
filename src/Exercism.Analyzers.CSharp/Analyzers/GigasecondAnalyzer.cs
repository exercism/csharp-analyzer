using static Exercism.Analyzers.CSharp.Analyzers.GigasecondSolutions;
using static Exercism.Analyzers.CSharp.Analyzers.GigasecondComments;
using static Exercism.Analyzers.CSharp.Analyzers.DefaultComments;

namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class GigasecondAnalyzer
    {
        public static SolutionAnalysis Analyze(CompiledSolution compiledSolution)
        {
            if (compiledSolution.IsEquivalentTo(AddSecondsWithScientificNotationInExpressionBody))
                return compiledSolution.ApproveAsOptimal();

            if (compiledSolution.IsEquivalentTo(AddSecondsWithScientificNotationInBlockBody))
                return compiledSolution.ApproveWithComment(UseExpressionBodiedMember);

            if (compiledSolution.IsEquivalentTo(AddSecondsWithMathPowInExpressionBody) || 
                compiledSolution.IsEquivalentTo(AddSecondsWithMathPowInBlockBody))
                return compiledSolution.ApproveWithComment(UseScientificNotationNotMathPow);

            if (compiledSolution.IsEquivalentTo(AddSecondsWithDigitsWithoutSeparatorInExpressionBody) || 
                compiledSolution.IsEquivalentTo(AddSecondsWithDigitsWithoutSeparatorInBlockBody))
                return compiledSolution.ApproveWithComment(UseScientificNotationOrDigitSeparators);

            if (compiledSolution.IsEquivalentTo(AddInExpressionBody) || 
                compiledSolution.IsEquivalentTo(AddInBlockBody) || 
                compiledSolution.IsEquivalentTo(PlusOperatorInExpressionBody) || 
                compiledSolution.IsEquivalentTo(PlusOperatorInBlockBody))
                return compiledSolution.DisapproveWithComment(UseAddSeconds);

            return compiledSolution.ReferToMentor();
        }
    }
}