using static Exercism.Analyzers.CSharp.Analyzers.GigasecondSolutions;

namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class GigasecondAnalyzer
    {
        public static AnalyzedSolution Analyze(SolutionImplementation implementation)
        {
            if (implementation.IsEquivalentTo(AddSecondsWithScientificNotation))
                return implementation.ApproveAsOptimal();

            if (implementation.IsEquivalentTo(AddSecondsWithMathPow))
                return implementation.ApproveWithComment("Use 1e9 instead of Math.Pow(10, 9)");

            if (implementation.IsEquivalentTo(AddSecondsWithDigitsWithoutSeparator))
                return implementation.ApproveWithComment("Use 1e9 or 1_000_000 instead of 1000000");

            if (implementation.IsEquivalentTo(AddSecondsWithScientificNotationInBlockBody))
                return implementation.ApproveWithComment("You could write the method an an expression-bodied member");

            if (implementation.IsEquivalentTo(Add))
                return implementation.DisapproveWithComment("Use AddSeconds");

            if (implementation.IsEquivalentTo(PlusOperator))
                return implementation.DisapproveWithComment("Use AddSeconds");

            return implementation.ReferToMentor();
        }
    }
}