using static Exercism.Analyzers.CSharp.Analyzers.GigasecondSolutions;

namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class GigasecondAnalyzer
    {
        public static SolutionAnalysis Analyze(SolutionImplementation solution)
        {
            if (solution.IsEquivalentTo(AddSecondsWithScientificNotation))
                return solution.ApproveAsOptimal();

            if (solution.IsEquivalentTo(AddSecondsWithMathPow))
                return solution.ApproveWithComment("Use 1e9 instead of Math.Pow(10, 9)");

            if (solution.IsEquivalentTo(AddSecondsWithDigitsWithoutSeparator))
                return solution.ApproveWithComment("Use 1e9 or 1_000_000 instead of 1000000");

            if (solution.IsEquivalentTo(AddSecondsWithScientificNotationInBlockBody))
                return solution.ApproveWithComment("You could write the method an an expression-bodied member");

            if (solution.IsEquivalentTo(Add))
                return solution.DisapproveWithComment("Use AddSeconds");

            if (solution.IsEquivalentTo(PlusOperator))
                return solution.DisapproveWithComment("Use AddSeconds");

            return solution.ReferToMentor();
        }
    }
}