using Serilog;
using static Exercism.Analyzers.CSharp.Analyzers.GigasecondSolutions;

namespace Exercism.Analyzers.CSharp.Analyzers
{
    public static class GigasecondAnalyzer
    {
        public static AnalyzedSolution Analyze(ImplementedSolution implementation)
        {
            Log.Information("Analysing {Exercise} using {Analyzer}",
                implementation.Solution.Exercise, nameof(GigasecondAnalyzer));

            if (implementation.IsEquivalentTo(AddSecondsWithScientificNotation))
                return new AnalyzedSolution(implementation.Solution, SolutionStatus.Approve);

            if (implementation.IsEquivalentTo(AddSecondsWithMathPow))
                return new AnalyzedSolution(implementation.Solution, SolutionStatus.Approve, "Use 1e9 instead of Math.Pow(10, 9)");

            if (implementation.IsEquivalentTo(AddSecondsWithDigitsWithoutSeparator))
                return new AnalyzedSolution(implementation.Solution, SolutionStatus.Approve, "Use 1e9 or 1_000_000 instead of 1000000");

            if (implementation.IsEquivalentTo(AddSecondsWithScientificNotationInBlockBody))
                return new AnalyzedSolution(implementation.Solution, SolutionStatus.Approve, "You could write the method an an expression-bodied member");

            if (implementation.IsEquivalentTo(Add))
                return new AnalyzedSolution(implementation.Solution, SolutionStatus.ReferToMentor, "Use AddSeconds");

            if (implementation.IsEquivalentTo(PlusOperator))
                return new AnalyzedSolution(implementation.Solution, SolutionStatus.ReferToMentor, "Use AddSeconds");

            return new AnalyzedSolution(implementation.Solution, SolutionStatus.ReferToMentor);
        }
    }
}