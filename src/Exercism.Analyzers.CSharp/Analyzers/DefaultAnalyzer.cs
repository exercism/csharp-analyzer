namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class DefaultAnalyzer
    {
        public static AnalyzedSolution Analyze(SolutionImplementation implementation) =>
            implementation.ReferToMentor();
    }
}