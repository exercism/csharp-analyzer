namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class DefaultAnalyzer
    {
        public static AnalyzedSolution Analyze(SolutionImplementation solutionImplementation) =>
            new AnalyzedSolution(solutionImplementation.Solution, SolutionStatus.ReferToMentor);
    }
}