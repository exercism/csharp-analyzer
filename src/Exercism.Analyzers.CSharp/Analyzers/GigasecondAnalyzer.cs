using static Exercism.Analyzers.CSharp.Analyzers.GigasecondSolutions;

namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class GigasecondAnalyzer
    {
        public static SolutionAnalysis Analyze(CompiledSolution compiledSolution)
        {
            if (compiledSolution.IsEquivalentTo(AddSecondsWithScientificNotationInExpressionBody))
                return compiledSolution.ApproveAsOptimal();

            if (compiledSolution.IsEquivalentTo(AddSecondsWithScientificNotationInBlockBody))
                return compiledSolution.ApproveWithComment("You could write the method an an expression-bodied member");

            if (compiledSolution.IsEquivalentTo(AddSecondsWithMathPowInExpressionBody))
                return compiledSolution.ApproveWithComment("Use 1e9 instead of Math.Pow(10, 9)");

            if (compiledSolution.IsEquivalentTo(AddSecondsWithMathPowInBlockBody))
                return compiledSolution.ApproveWithComment("Use 1e9 instead of Math.Pow(10, 9)");

            if (compiledSolution.IsEquivalentTo(AddSecondsWithDigitsWithoutSeparatorInExpressionBody))
                return compiledSolution.ApproveWithComment("Use 1e9 or 1_000_000 instead of 1000000");

            if (compiledSolution.IsEquivalentTo(AddSecondsWithDigitsWithoutSeparatorInBlockBody))
                return compiledSolution.ApproveWithComment("Use 1e9 or 1_000_000 instead of 1000000");

            if (compiledSolution.IsEquivalentTo(AddInExpressionBody))
                return compiledSolution.DisapproveWithComment("Use AddSeconds");

            if (compiledSolution.IsEquivalentTo(AddInBlockBody))
                return compiledSolution.DisapproveWithComment("Use AddSeconds");

            if (compiledSolution.IsEquivalentTo(PlusOperatorInExpressionBody))
                return compiledSolution.DisapproveWithComment("Use AddSeconds");

            if (compiledSolution.IsEquivalentTo(PlusOperatorInBlockBody))
                return compiledSolution.DisapproveWithComment("Use AddSeconds");

            return compiledSolution.ReferToMentor();
        }
    }
}