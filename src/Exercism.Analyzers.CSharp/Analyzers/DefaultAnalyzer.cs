namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class DefaultAnalyzer
    {
        public static SolutionAnalysis Analyze(SolutionImplementation solution) =>
            solution.ReferToMentor();
    }
}